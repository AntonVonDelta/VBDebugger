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

class TaskCompletionSource {
private:
	class TaskRegistration;

	std::mutex mtx_set_signal = {};
	std::atomic<bool> value;
	std::map<int, std::function<void(void)>> registeredCallbacks;
	int lastRegistrationId = 0;

	int InternalRegisterCallback(std::function<void(void)> callback);
	void InternalUnregisterCallback(int registrationId);

public:
	class TaskRegistration;

	static std::unique_ptr<TaskRegistration> RegisterCallback(std::shared_ptr<TaskCompletionSource> tcs, std::function<void(void)> callback);

	void SetResult();
};

