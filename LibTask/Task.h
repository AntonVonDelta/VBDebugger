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

#include "Utils.h"

namespace TPL {
	class BaseTask {
	public:
		virtual bool IsFinished() = 0;
	};

	/// <summary>
	/// The public interface
	/// </summary>
	template<typename T>
	class DLL_API Task :public BaseTask {
	public:
		virtual void Result() = 0;

		// Must be defined in order for Tasks to be compared
		virtual bool operator==(const Task& other) = 0;
	};
}
