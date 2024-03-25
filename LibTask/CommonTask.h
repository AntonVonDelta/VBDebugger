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

#include "Task.h"

namespace TPL {
	class InternalTaskData {
	private:
		friend class TaskRegistration;

		template<typename T>
		friend class CommonTask;

		int lastRegistrationId = 0;

		// Move members from private to public as they become needed

	public:
		std::mutex mtxSync;

		// Used by TaskCompletionSource
		std::map<int, std::function<void(void)>> registeredCallbacks;
		std::unordered_set<std::shared_ptr<std::condition_variable>> registeredNotificationSignals;
	};


	class TaskRegistration {
	private:
		std::shared_ptr<InternalTaskData> source;
		int id;

	public:
		TaskRegistration(std::shared_ptr<InternalTaskData> source, int id);
		~TaskRegistration();
	};


	/// <summary>
	/// Minimum functionality class for tasks.
	/// The library depends on the premise that all tasks actualy inherit this
	/// </summary>
	template<typename T>
	class CommonTask :public TPL::Task<T> {
	private:
		int Register(std::function<void(void)> callback);

	public:

		/// <summary>
		/// Stored the data in a separate structure
		/// because this allows us to copy the parent class while keeping uncopiable data the same.
		/// (mutexes, conditional variables, etc.)
		/// Also this allows us to call code from RAII classes like Registration
		/// that on destruction try to access the parent class data. 
		/// Previously doing this could not guarantee that no use after free would hapen
		/// </summary>
		std::shared_ptr<InternalTaskData> data;

		CommonTask();
		CommonTask(std::shared_ptr<InternalTaskData> data);

		virtual std::unique_ptr<TaskRegistration> RegisterCallback(std::function<void(void)> callback);
		virtual void AddNotificationSignal(std::shared_ptr<std::condition_variable> conditional);
		virtual void RemoveNotificationSignal(std::shared_ptr<std::condition_variable> conditional);

		virtual ~CommonTask();
	};
}

