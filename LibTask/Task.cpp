#include "Task.h"

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

int TPL::InternalTaskData::Register(std::function<void(void)> callback) {
	auto shouldCall = false;
	int currentRegistrationId = 0;

	{
		std::scoped_lock lock(mtxSync);

		lastRegistrationId++;
		currentRegistrationId = lastRegistrationId;

		registeredCallbacks[currentRegistrationId] = callback;

		if (value)
			shouldCall = true;
	}

	if (shouldCall) {
		try {
			callback();
		} catch (std::exception) {}
	}

	return currentRegistrationId;
}
void TPL::InternalTaskData::Unregister(int registrationId) {
	std::scoped_lock lock(mtxSync);

	registeredCallbacks.erase(registrationId);
}

std::unique_ptr<TPL::TaskRegistration> TPL::InternalTask::RegisterCallback(std::function<void(void)> callback) {
	int registrationId = data->Register(callback);

	return std::make_unique<TaskRegistration>(data, registrationId);
}
void TPL::InternalTask::AddNotificationSignal(std::shared_ptr<std::condition_variable> conditional) {
	std::scoped_lock lock(data->mtxSync);

	data->registeredNotificationSignals.insert(conditional);
}
void TPL::InternalTask::RemoveNotificationSignal(std::shared_ptr<std::condition_variable> conditional) {
	std::scoped_lock lock(data->mtxSync);

	data->registeredNotificationSignals.erase(conditional);
}
TPL::InternalTask::~InternalTask() {}


