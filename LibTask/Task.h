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
	class TaskRegistration {
	private:
		std::shared_ptr<InternalTask> source;
		int id;

	public:
		TaskRegistration(std::shared_ptr<InternalTask> source, int id) {
			this->source = source;
			this->id = id;
		}

		~TaskRegistration() {
			source->InternalUnregisterCallback(id);
		}
	};

	struct InternalTaskData {
		std::atomic<bool> value;
		std::map<int, std::function<void(void)>> registeredCallbacks;
		int lastRegistrationId = 0;

		std::mutex mtxSync;
		std::unordered_set<std::shared_ptr<std::condition_variable>> registeredNotificationSignals;
	};

	class InternalTask {
	private:
		friend class TaskRegistration;

		std::shared_ptr<InternalTaskData> data;

		int InternalRegisterCallback(std::function<void(void)> callback);
		void InternalUnregisterCallback(int registrationId);
		std::unique_ptr<TaskRegistration> RegisterCallback(std::function<void(void)> callback);

	protected:

	public:
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
