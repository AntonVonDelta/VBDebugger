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
#include "Task.h"

TaskCompletionSource::TaskCompletionSource() {
	Task = std::make_shared<SourceTask>(this);
}

void TaskCompletionSource::SetResult() {
	auto castedTask = (SourceTask*)Task.get();

	{
		std::scoped_lock lock(castedTask->data->mtxSync);

		if (castedTask->data->value == true)
			throw std::exception("TaskCompletionSource already set");

		castedTask->data->value = true;
	}

	castedTask->InternalSignalCompleted();
}

TaskCompletionSource::SourceTask::SourceTask(TaskCompletionSource* tcs) {
	this->tcs = tcs;
}

void TaskCompletionSource::SourceTask::InternalSignalCompleted() {
	for (const auto& conditional : data->registeredNotificationSignals) {
		conditional->notify_all();
	}

	for (const auto& callbackPair : data->registeredCallbacks) {
		try {
			callbackPair.second();
		} catch (std::exception) {}
	}
}

void TaskCompletionSource::SourceTask::Result() {
	data->value.wait(false);
}

bool TaskCompletionSource::SourceTask::IsFinished() {
	return data->value;
}
