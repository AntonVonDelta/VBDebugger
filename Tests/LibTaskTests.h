#pragma once

#include <iostream>
#include <thread>
#include <chrono>
#include <string>

#include "../LibTask/Task.h"
#include "../LibTask/TaskCompletionSource.h"
#include "../LibTask/Extensions.h"

void libtask_run1() {
	TaskCompletionSource tcs;

	std::cout << "TCS SetResult()\n";
	tcs.SetResult();

	std::cout << "TCS->Task Result()\n";
	tcs.Task->Result();
	std::cout << "TCS->Task Result() finished\n";
}


void libtask_run2() {
	TaskCompletionSource tcs;

	auto& task = tcs.Task;

	auto t1 = std::jthread([&] {
		std::cout << "TCS Delay before setting tcs\n";
		std::this_thread::sleep_for(std::chrono::seconds(2));

		std::cout << "TCS SetResult()\n";
		tcs.SetResult();
		});

	std::cout << "TCS->Task Result() before\n";
	task->Result();
	std::cout << "TCS->Task Result() finished\n";
}

void libtask_run3() {
	TaskCompletionSource tcs1;
	TaskCompletionSource tcs2;

	auto task1 = tcs1.Task;
	auto task2 = tcs2.Task;

	auto t1 = std::jthread([&] {
		std::this_thread::sleep_for(std::chrono::seconds(1));

		std::cout << "TCS 1 SetResult()\n";
		tcs1.SetResult();
		});

	auto t2 = std::jthread([&] {
		std::this_thread::sleep_for(std::chrono::seconds(3));

		std::cout << "TCS 2 SetResult()\n";
		tcs2.SetResult();
		});

	std::cout << "WhenAny before\n";
	auto task3 = TPL::WhenAny({ task1,task2 });

	task3->Result();
	std::cout << "WhenAny finished\n";
}

void libtask_run4() {
	TaskCompletionSource tcs1;
	TaskCompletionSource tcs2;

	auto task1 = tcs1.Task;
	auto task2 = tcs2.Task;

	auto t1 = std::jthread([&] {
		std::this_thread::sleep_for(std::chrono::seconds(1));

		std::cout << "TCS 1 SetResult()\n";
		tcs1.SetResult();
		});

	auto t2 = std::jthread([&] {
		std::this_thread::sleep_for(std::chrono::seconds(3));

		std::cout << "TCS 2 SetResult()\n";
		tcs2.SetResult();
		});

	std::cout << "WhenAll before\n";
	auto task3 = TPL::WhenAll({ task1,task2 });

	task3->Result();
	std::cout << "WhenAll finished\n";
}