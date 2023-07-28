#pragma once

#include <mutex>
#include <memory>
#include <atomic>
#include <condition_variable>
#include <functional>
#include <future>
#include <chrono>

class TwoWaySynchronization {
private:
	std::mutex mtx_sync = {};
	std::condition_variable signal_ping = {};
	std::condition_variable signal_pong = {};
	std::atomic<bool> state = false;

public:
	/// <summary>
	/// Useful for avoiding blocking on Read
	/// </summary>
	bool IsDataAvailable() {
		return state.load();
	}

	void SendOne(std::atomic<bool>& cancel_token) {
		std::unique_lock<std::mutex> lock_signal(mtx_sync);

		state = true;
		signal_ping.notify_all();

		while (!cancel_token) {
			auto result = signal_pong.wait_for(lock_signal, std::chrono::seconds(2), [this]() {return !state.load(); });
			if (result) break;
		}
	}

	void ReadOne() {
		std::unique_lock<std::mutex> lock_signal(mtx_sync);

		signal_ping.wait(lock_signal, [this]() {return state.load(); });

		state = false;
		signal_pong.notify_all();
	}
};

