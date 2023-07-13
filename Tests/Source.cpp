#include "MessageBroker.h"
#include "Breakpoint.h"
#include <iostream>
#include <thread>
#include <chrono>

using namespace std;

enum Message {
	Test1,
	Hello
};

MessageBroker<Message> broker;
Breakpoint breakpoint;

struct Temp {
	int a;
};

struct TempT {
	typedef Temp Type;
};

void run() {
	int i = 0;

	while (true) {
		i++;

		cout << "before breakpoint " << endl;

		if (breakpoint.input()) {
			cout << "the instruction at " << i << endl;
		}
	}
}


template<typename T>
int testTemp() {
	typename T::Type test = {};

	int b = test.a;

	return b;
}

int main() {
	cout << testTemp<TempT>();


	return 0;
	auto t1 = thread(&run);

	std::this_thread::sleep_for(std::chrono::seconds(5));

	while (true) {
		int choice;

		cout << "Choice: ";
		cin >> choice;

		switch (choice) {
			case 0:
				breakpoint.pause();
				break;

			case 1:
				breakpoint.resume();
				break;

			case 2:
				breakpoint.stepOver();
				break;

			case 3:
				breakpoint.stepOver(false);
				break;
		}
	}

	t1.join();
}