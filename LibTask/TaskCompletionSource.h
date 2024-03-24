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

class __declspec(dllexport) TaskCompletionSource {
public:
	TaskCompletionSource();

public:
	std::shared_ptr<TPL::Task> Task;

	void SetResult();
};

