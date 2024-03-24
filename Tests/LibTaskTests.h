#pragma once

#include <iostream>
#include <thread>
#include <chrono>
#include <string>

#include "../LibTask/Task.h"
#include "../LibTask/TaskCompletionSource.h"

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

	auto& temp = tcs.Task;


	auto t1 = std::thread([&] {
		std::cout << "TCS Delay before setting tcs\n";

		std::this_thread::sleep_for(std::chrono::seconds(5));

		std::cout << "TCS SetResult()\n";
		tcs.SetResult();

		temp->Result();

		});

	std::cout << "TCS->Task Result()\n";
	temp->Result();
	std::cout << "TCS->Task Result() finished\n";

	t1.join();
}