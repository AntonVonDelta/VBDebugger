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

#include "TaskCompletionSource.h"
#include "CommonTask.h"

template<typename T>
class ComplexTaskData :public TPL::InternalTaskData {
public:
	std::atomic<T> value;
};

template<typename T>
class SourceTask :public TPL::CommonTask<T> {
private:
	friend class TaskCompletionSource<T>;
	TaskCompletionSource<T>* tcs;

	void InternalSignalCompleted() {
		for (const auto& conditional : (TPL::CommonTask<T>::data)->registeredNotificationSignals) {
			conditional->notify_all();
		}

		for (const auto& callbackPair : (TPL::CommonTask<T>::data)->registeredCallbacks) {
			try {
				callbackPair.second();
			} catch (std::exception) {}
		}
	}

public:
	SourceTask(TaskCompletionSource<T>* tcs, T value) :CommonTask<T>(std::make_shared<ComplexTaskData>()) {
		this->tcs = tcs;
	}

	T Result() override {
		auto& castedData = static_cast<ComplexTaskData<T>&>(*CommonTask<T>::data);

		castedData.value.wait(false);

		return castedData.value;
	}
	bool IsFinished() override {
		auto& castedData = static_cast<ComplexTaskData<T>&>(*CommonTask<T>::data);

		return castedData.value;
	}
};


class SimpleTaskData :public TPL::InternalTaskData {
public:
	std::atomic<bool> value;
};

template<>
class SourceTask<void> :public TPL::CommonTask<void> {
private:
	friend class TaskCompletionSource<void>;
	TaskCompletionSource<void>* tcs;

	void InternalSignalCompleted() {
		for (const auto& conditional : (TPL::CommonTask<void>::data)->registeredNotificationSignals) {
			conditional->notify_all();
		}

		for (const auto& callbackPair : (TPL::CommonTask<void>::data)->registeredCallbacks) {
			try {
				callbackPair.second();
			} catch (std::exception) {}
		}
	}

public:
	SourceTask(TaskCompletionSource<void>* tcs) :CommonTask<void>(std::make_shared<SimpleTaskData>()) {
		this->tcs = tcs;
	}

	void Result() override {
		auto& castedData = static_cast<SimpleTaskData&>(*CommonTask<void>::data);

		castedData.value.wait(false);
	}
	bool IsFinished() override {
		auto& castedData = static_cast<SimpleTaskData&>(*CommonTask<void>::data);

		return castedData.value;
	}
};




template<typename T>
TaskCompletionSource<T>::TaskCompletionSource() {
	Task = std::make_shared<SourceTask<T>>(this);
}

template<typename T>
void TaskCompletionSource<T>::SetResult(T value) {
	auto& castedTask = static_cast<SourceTask<T>&>(*Task);

	{
		std::scoped_lock lock(castedTask.data->mtxSync);

		auto& castedData = static_cast<SimpleTaskData&>(*castedTask.data);

		if (castedData.value == true)
			throw std::exception("TaskCompletionSource already set");

		castedData.value = true;
		castedData.value.notify_all();
	}

	castedTask.InternalSignalCompleted();
}



TaskCompletionSource<void>::TaskCompletionSource() {
	Task = std::make_shared<SourceTask<void>>(this);
}

void TaskCompletionSource<void>::SetResult() {
	auto& castedTask = static_cast<SourceTask<void>&>(*Task);

	{
		std::scoped_lock lock(castedTask.data->mtxSync);

		auto& castedData = static_cast<SimpleTaskData&>(*castedTask.data);

		if (castedData.value == true)
			throw std::exception("TaskCompletionSource already set");

		castedData.value = true;
		castedData.value.notify_all();
	}

	castedTask.InternalSignalCompleted();
}
