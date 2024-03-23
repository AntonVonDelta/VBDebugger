#include "TaskCompletionSource.h"

class TaskCompletionSource::TaskRegistration {
private:
	std::shared_ptr<TaskCompletionSource> source;
	int id;

	TaskRegistration(std::shared_ptr<TaskCompletionSource> source, int id) {
		this->source = source;
		this->id = id;
	}

	~TaskRegistration() {
		source->InternalUnregisterCallback(id);
	}
};


int TaskCompletionSource::InternalRegisterCallback(std::function<void(void)> callback) {
	auto shouldCall = false;
	int currentRegistrationId = 0;

	{
		std::scoped_lock lock(mtx_set_signal);

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

void TaskCompletionSource::InternalUnregisterCallback(int registrationId) {
	std::scoped_lock lock(mtx_set_signal);

	registeredCallbacks.erase(registrationId);
}

std::unique_ptr<TaskCompletionSource::TaskRegistration> TaskCompletionSource::RegisterCallback(std::shared_ptr<TaskCompletionSource> tcs, std::function<void(void)> callback) {
	int registrationId = tcs->InternalRegisterCallback(callback);

	return std::make_unique<TaskRegistration>(tcs, registrationId);
}

void TaskCompletionSource::SetResult() {
	std::scoped_lock lock(mtx_set_signal);
	value = true;
}