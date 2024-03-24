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

namespace TPL {
	class InternalTaskData {
	private:
		friend class TaskRegistration;
		friend class InternalTask;

		// Move members from private to public as they become needed

		std::map<int, std::function<void(void)>> registeredCallbacks;
		int lastRegistrationId = 0;


		int Register(std::function<void(void)> callback);
		void Unregister(int registrationId);

	public:
		std::mutex mtxSync;

		// Used by TaskCompletionSource
		std::atomic<bool> value;
		std::unordered_set<std::shared_ptr<std::condition_variable>> registeredNotificationSignals;
	};


	class TaskRegistration {
	private:
		std::shared_ptr<InternalTaskData> source;
		int id;

	public:
		TaskRegistration(std::shared_ptr<InternalTaskData> source, int id) {
			this->source = source;
			this->id = id;
		}

		~TaskRegistration() {
			source->Unregister(id);
		}
	};


	class InternalTask {
	protected:
		/// <summary>
		/// Stored the data in a separate structure
		/// because this allows us to copy the parent class while keeping uncopiable data the same.
		/// (mutexes, conditional variables, etc.)
		/// Also this allows us to call code from RAII classes like Registration
		/// that on destruction try to access the parent class data. 
		/// Previously doing this could not guarantee that no use after free would hapen
		/// </summary>
		std::shared_ptr<InternalTaskData> data;

	public:
		std::unique_ptr<TaskRegistration> RegisterCallback(std::function<void(void)> callback);
		void AddNotificationSignal(std::shared_ptr<std::condition_variable> conditional);
		void RemoveNotificationSignal(std::shared_ptr<std::condition_variable> conditional);

		virtual ~InternalTask();
	};



	class Task :public InternalTask {
	public:
		virtual void Result() = 0;
		virtual bool IsFinished() = 0;
	};
}
