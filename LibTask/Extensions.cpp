#include <mutex>
#include <utility>
#include <atomic>
#include <condition_variable>
#include <memory>
#include <vector>
#include <functional>
#include <exception>
#include <map>

#include "Extensions.h"
#include "Task.h"

class InternalAnyTask :public TPL::Task {
private:
	std::vector<std::shared_ptr<TPL::Task>> tasks;
	std::shared_ptr<std::condition_variable> setSignal;

	bool IsAnyFinished() {
		for (const auto& task : tasks) {
			if (task->IsFinished())
				return true;
		}
		return false;
	}

public:

	InternalAnyTask(std::vector<std::shared_ptr<TPL::Task>> tasks) {
		// The given parameters are stored locally in order to keep them referenced
		// in memory for the entire lifetime of this object
		this->tasks = tasks;

		setSignal = std::make_shared<std::condition_variable>();

		for (const auto& task : tasks) {
			TPL::InternalTask* castedTask = static_cast<TPL::InternalTask*>(task.get());
			task->AddNotificationSignal(setSignal);
		}
	}

	void Result() override {
		std::unique_lock<std::mutex> lock(data->mtxSync);

		setSignal->wait(lock,
			[this]() {
				return IsAnyFinished();
			});
	}



	bool IsFinished() override {
		return IsAnyFinished();
	}

	~InternalAnyTask() {
		for (const auto& task : tasks) {
			TPL::InternalTask* castedTask = static_cast<TPL::InternalTask*>(task.get());
			task->RemoveNotificationSignal(setSignal);
		}
	}
};

std::unique_ptr<TPL::Task> WhenAny(std::vector<std::shared_ptr<TPL::Task>> tasks) {
	return std::make_unique<InternalAnyTask>(tasks);
}
