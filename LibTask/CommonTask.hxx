#pragma once

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
#include "CommonTask.h"

template<typename T>
int TPL::CommonTask<T>::Register(std::function<void(void)> callback) {
	auto shouldCall = false;
	int currentRegistrationId = 0;

	{
		std::scoped_lock lock(data->mtxSync);

		data->lastRegistrationId++;
		currentRegistrationId = data->lastRegistrationId;

		data->registeredCallbacks[currentRegistrationId] = callback;

		if (this->IsFinished())
			shouldCall = true;
	}

	if (shouldCall) {
		try {
			callback();
		} catch (std::exception) {}
	}

	return currentRegistrationId;
}

template<typename T>
TPL::CommonTask<T>::CommonTask() {
	data = std::make_shared<InternalTaskData>();
}

template<typename T>
TPL::CommonTask<T>::CommonTask(std::shared_ptr<InternalTaskData> data) {
	this->data = data;
}

template<typename T>
std::unique_ptr<TPL::TaskRegistration> TPL::CommonTask<T>::RegisterCallback(std::function<void(void)> callback) {
	int registrationId = Register(callback);

	return std::make_unique<TaskRegistration>(data, registrationId);
}

template<typename T>
void TPL::CommonTask<T>::AddNotificationSignal(std::shared_ptr<std::condition_variable> conditional) {
	std::scoped_lock lock(data->mtxSync);

	data->registeredNotificationSignals.insert(conditional);
}

template<typename T>
void TPL::CommonTask<T>::RemoveNotificationSignal(std::shared_ptr<std::condition_variable> conditional) {
	std::scoped_lock lock(data->mtxSync);

	data->registeredNotificationSignals.erase(conditional);
}

template<typename T>
TPL::CommonTask<T>::~CommonTask() {}