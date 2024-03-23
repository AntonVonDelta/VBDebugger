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

class Task {
private:
	std::mutex mtxSync;
	std::atomic<bool> value;
	std::unordered_set<std::shared_ptr<std::condition_variable>> registeredNotificationSignals;

	void AddNotificationSignal(std::shared_ptr<std::condition_variable> conditional);
	void RemoveNotificationSignal(std::shared_ptr<std::condition_variable> conditional);

	void SetResult();

public:

	bool IsFinished();
};

