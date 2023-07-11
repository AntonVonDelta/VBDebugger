#pragma once

#include "MessageResponse.h"
#include <memory>
#include <optional>
#include <future>
#include <mutex>
#include <condition_variable>
#include <exception>

typedef std::promise<std::unique_ptr<MessageResponse>> MESSAGE_PROMISE;
typedef std::future<std::unique_ptr<MessageResponse>> MESSAGE_FUTURE;


template<typename T>
class MessageBroker {
private:
	std::mutex mtx_conditional = {};
	std::condition_variable signal = {};
	std::promise<std::unique_ptr<MessageResponse>> promise;
	bool new_message = false;
	T message;

public:

	std::optional<MESSAGE_PROMISE> getMessage(T& message, bool wait) {
		return internalGetMessage(message, wait);
	}

	MESSAGE_FUTURE sendMessage(T message) {
		std::unique_lock<std::mutex> lock(mtx_conditional);
		MESSAGE_FUTURE temp_future;

		if (new_message)
			throw std::runtime_error("Cannot send a new message because the previous one was not processed");

		new_message = true;

		this->message = message;
		promise = MESSAGE_PROMISE();

		// We move the future to a local variable
		// because after we release the lock, promise will be set empty
		// by getMessage
		temp_future = promise.get_future();

		// Unlock is allowed here
		// Used to prevent the other wait from waking up and 
		// blocking a second time trying to acquire this lock
		lock.unlock();

		signal.notify_all();

		return temp_future;
	}

private:
	std::optional<MESSAGE_PROMISE> internalGetMessage(T& message, bool wait) {
		// Use locking to wait for a message to be published
		std::unique_lock<std::mutex> lock(mtx_conditional);

		if (!wait && !new_message) return {};

		// Wait for signal
		signal.wait(lock, [this]() {return new_message; });

		// Reset flag
		new_message = false;

		// Return data
		message = this->message;

		// Can't return directly promise because 
		// it seems for member values no move operator is used
		// but copy instead which fails
		return std::move(promise);
	}
};

