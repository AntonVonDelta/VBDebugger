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

class DLL_API TaskCompletionSource {
public:
	TaskCompletionSource();

public:
	std::shared_ptr<TPL::Task> Task;

	void SetResult();
};

