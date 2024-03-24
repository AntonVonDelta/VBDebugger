#include <mutex>
#include <utility>
#include <atomic>
#include <condition_variable>
#include <memory>
#include <vector>
#include <functional>
#include <exception>
#include <map>

#include "TaskCompletionSource.h"
#include "CommonTask.h"

class SimpleTaskData :public TPL::InternalTaskData {
public:
	std::atomic<bool> value;
};

class SourceTask :public TPL::CommonTask {
private:
	friend class TaskCompletionSource;
	TaskCompletionSource* tcs;

	void InternalSignalCompleted() {
		for (const auto& conditional : data->registeredNotificationSignals) {
			conditional->notify_all();
		}

		for (const auto& callbackPair : data->registeredCallbacks) {
			try {
				callbackPair.second();
			} catch (std::exception) {}
		}
	}

public:
	SourceTask(TaskCompletionSource* tcs) :CommonTask(std::make_shared<SimpleTaskData>()) {
		this->tcs = tcs;
	}

	void Result() override {
		auto& castedData = static_cast<SimpleTaskData&>(*data);

		castedData.value.wait(false);
	}
	bool IsFinished() override {
		auto& castedData = static_cast<SimpleTaskData&>(*data);

		return castedData.value;
	}
};


TaskCompletionSource::TaskCompletionSource() {
	Task = std::make_shared<SourceTask>(this);
}

void TaskCompletionSource::SetResult() {
	auto& castedTask = static_cast<SourceTask&>(*Task);

	{
		std::scoped_lock lock(castedTask.data->mtxSync);

		auto& castedData = static_cast<SimpleTaskData&>(*castedTask.data);

		if (castedData.value == true)
			throw std::exception("TaskCompletionSource already set");

		castedData.value = true;
		castedData.value.notify_all();
	}

	castedTask.InternalSignalCompleted();
}
