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

int TPL::InternalTask::InternalRegisterCallback(std::function<void(void)> callback) {
	auto shouldCall = false;
	int currentRegistrationId = 0;

	{
		std::scoped_lock lock(mtxSetSignal);

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

void TPL::InternalTask::InternalUnregisterCallback(int registrationId) {
	std::scoped_lock lock(mtxSetSignal);

	registeredCallbacks.erase(registrationId);
}

std::unique_ptr<TPL::TaskRegistration> TPL::InternalTask::RegisterCallback(std::function<void(void)> callback) {
	int registrationId = InternalRegisterCallback(callback);

	return std::make_unique<TaskRegistration>(registrationId);
}


void TPL::InternalTask::AddNotificationSignal(std::shared_ptr<std::condition_variable> conditional) {
	std::scoped_lock lock(mtxSync);

	registeredNotificationSignals.insert(conditional);
}

void TPL::InternalTask::RemoveNotificationSignal(std::shared_ptr<std::condition_variable> conditional) {
	std::scoped_lock lock(mtxSync);

	registeredNotificationSignals.erase(conditional);
}
TPL::InternalTask::~InternalTask() {}

