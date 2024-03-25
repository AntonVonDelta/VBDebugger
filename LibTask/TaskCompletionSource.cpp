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

template<typename T>
class SourceTask :public TPL::CommonTask<T> {
private:
	friend class TaskCompletionSource<T>;
	TaskCompletionSource<T>* tcs;

	void InternalSignalCompleted() {
		for (const auto& conditional : (TPL::CommonTask<T>::data)->registeredNotificationSignals) {
			conditional->notify_all();
		}

		for (const auto& callbackPair : (TPL::CommonTask<T>::data)->registeredCallbacks) {
			try {
				callbackPair.second();
			} catch (std::exception) {}
		}
	}

public:
	SourceTask(TaskCompletionSource<T>* tcs) :CommonTask<T>(std::make_shared<SimpleTaskData>()) {
		this->tcs = tcs;
	}

	void Result() override {
		auto& castedData = static_cast<SimpleTaskData&>(*CommonTask<T>::data);

		castedData.value.wait(false);
	}
	bool IsFinished() override {
		auto& castedData = static_cast<SimpleTaskData&>(*CommonTask<T>::data);

		return castedData.value;
	}
};

template<typename T>
TaskCompletionSource<T>::TaskCompletionSource<T>() {
	Task = std::make_shared<SourceTask<T>>(this);
}

template<typename T>
void TaskCompletionSource<T>::SetResult(T value) {
	auto& castedTask = static_cast<SourceTask<T>&>(*Task);

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
