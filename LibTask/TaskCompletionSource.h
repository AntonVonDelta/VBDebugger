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
#include "Task.h"

class TaskCompletionSource {
private:
	std::atomic<bool> value;

	class InternalTask :Task {
	private:
		friend class TaskCompletionSource;
		TaskCompletionSource* tcs;

		
		InternalTask(TaskCompletionSource* tcs);
		
		void InternalSignalCompleted();

	public:
		void Result() override;
		bool IsFinished() override;
	};

public:
	TaskCompletionSource();

public:
	std::shared_ptr<Task> Task;
	
	void SetResult();
};

