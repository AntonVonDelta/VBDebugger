#pragma once

#include <mutex>
#include <memory>
#include <atomic>
#include <functional>
#include <vector>
#include <map>

class CancellationToken {
private:
	std::atomic<bool> cancelled = false;
	std::map<int, std::function<void()>> callbacks;
	int last_unique_id = 0;

public:
	bool IsCancellationRequested() {
		return cancelled;
	}

	void Cancel() {
		if (cancelled) return;
		cancelled = true;

		for (const auto& callback_pair : callbacks) {
			callback_pair.second();
		}
	}

	int Register(std::function<void()> callback) {
		last_unique_id++;

		callbacks[last_unique_id] = callback;

		if (cancelled) callback();

		return last_unique_id;
	}

	void Unregister(int id) {
		auto iter = callbacks.find(id);

		if (iter != callbacks.end())
			callbacks.erase(iter);
	}
};

