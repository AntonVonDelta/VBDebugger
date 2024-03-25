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
#include "Utils.h"

template<typename T>
class TaskCompletionSource {
public:
	TaskCompletionSource();

public:
	std::shared_ptr<TPL::Task<T>> Task;

	void SetResult(T value);
};

template<>
class TaskCompletionSource<void> {
public:
	TaskCompletionSource();

public:
	std::shared_ptr<TPL::Task<void>> Task;

	void SetResult();
};

#include "TaskCompletionSource.hxx"

