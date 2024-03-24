#include <mutex>
#include <utility>
#include <atomic>
#include <condition_variable>
#include <memory>
#include <vector>
#include <functional>
#include <exception>
#include <map>
#include <unordered_set>

#include "Task.h"


TPL::TaskRegistration::TaskRegistration(std::shared_ptr<InternalTaskData> source, int id) {
	this->source = source;
	this->id = id;
}

TPL::TaskRegistration::~TaskRegistration() {
	std::scoped_lock lock(source->mtxSync);

	source->registeredCallbacks.erase(id);
}



TPL::CommonTask::CommonTask() {
	data = std::make_shared<InternalTaskData>();
}

TPL::CommonTask::CommonTask(std::shared_ptr<InternalTaskData> data) {
	this->data = data;
}

int TPL::CommonTask::Register(std::function<void(void)> callback) {
	auto shouldCall = false;
	int currentRegistrationId = 0;

	{
		std::scoped_lock lock(data->mtxSync);

		data->lastRegistrationId++;
		currentRegistrationId = data->lastRegistrationId;

		data->registeredCallbacks[currentRegistrationId] = callback;

		if (IsFinished())
			shouldCall = true;
	}

	if (shouldCall) {
		try {
			callback();
		} catch (std::exception) {}
	}

	return currentRegistrationId;
}

std::unique_ptr<TPL::TaskRegistration> TPL::CommonTask::RegisterCallback(std::function<void(void)> callback) {
	int registrationId = Register(callback);

	return std::make_unique<TaskRegistration>(data, registrationId);
}

void TPL::CommonTask::AddNotificationSignal(std::shared_ptr<std::condition_variable> conditional) {
	std::scoped_lock lock(data->mtxSync);

	data->registeredNotificationSignals.insert(conditional);
}

void TPL::CommonTask::RemoveNotificationSignal(std::shared_ptr<std::condition_variable> conditional) {
	std::scoped_lock lock(data->mtxSync);

	data->registeredNotificationSignals.erase(conditional);
}

bool TPL::CommonTask::operator==(const Task& other) {
	const CommonTask* castedOther = dynamic_cast<const CommonTask*>(&other);

	return data == castedOther->data;
}

TPL::CommonTask::~CommonTask() {}


