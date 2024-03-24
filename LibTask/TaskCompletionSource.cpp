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
	Task = std::make_shared<InternalTask>(this);
}

void TaskCompletionSource::SetResult() {
	Task->data;
	//->value = true;

	auto internalTask = (InternalTask*)(Task.get());

	internalTask->InternalSignalCompleted();
}

TaskCompletionSource::InternalTask::InternalTask(TaskCompletionSource* tcs) {
	this->tcs = tcs;
}

void TaskCompletionSource::InternalTask::InternalSignalCompleted() {
	std::scoped_lock lock(data->mtxSync);

	for (const auto& conditional : data->registeredNotificationSignals) {
		conditional->notify_all();
	}
}

void TaskCompletionSource::InternalTask::Result() {
	tcs->value.wait(false);
}

bool TaskCompletionSource::InternalTask::IsFinished() {
	return tcs->value;
}
