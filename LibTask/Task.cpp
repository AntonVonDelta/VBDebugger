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

void Task::AddNotificationSignal(std::shared_ptr<std::condition_variable> conditional) {
	std::scoped_lock lock(mtxSync);

	registeredNotificationSignals.insert(conditional);
}

void Task::RemoveNotificationSignal(std::shared_ptr<std::condition_variable> conditional) {
	std::scoped_lock lock(mtxSync);

	registeredNotificationSignals.erase(conditional);
}

void Task::SetResult() {
	std::scoped_lock lock(mtxSync);

	value = true;

	for (const auto& notification : registeredNotificationSignals) {
		notification->notify_all();
	}
}

bool Task::IsFinished() {
	return value;
}