#include "MessageBroker.h"
#include "Breakpoint.h"
#include <Windows.h>
#include <iostream>
#include <thread>
#include <chrono>

using namespace std;

void WINAPI Init();
void WINAPI Log(const char* filename, const char* scope_name, int line_number, const char* arguments);
void WINAPI EnterProcedure(const char* filename, const  char* scope_name, int line_number, const char* arguments);
void WINAPI LeaveProcedure(const char* filename, const  char* scope_name, int line_number);


Breakpoint breakpoint;

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


void run2() {
	int i = 0;

	EnterProcedure("test", "scope", __LINE__, "none");

	while (true) {
		i++;

		cout << "before breakpoint " << endl;

		Log("test", "scope", __LINE__, "none");
		cout << "the instruction at " << i << endl;

		std::this_thread::sleep_for(std::chrono::milliseconds(100));
	}

	LeaveProcedure("test", "scope", __LINE__);
}



int main() {
	Init();

	auto t1 = thread(&run2);

	std::this_thread::sleep_for(std::chrono::seconds(5));

	//while (true) {
	//	int choice;

	//	cout << "Choice: ";
	//	cin >> choice;

	//	switch (choice) {
	//		case 0:
	//			breakpoint.pause();
	//			break;

	//		case 1:
	//			breakpoint.resume();
	//			break;

	//		case 2:
	//			breakpoint.stepOver();
	//			break;

	//		case 3:
	//			breakpoint.stepOver(false);
	//			break;
	//	}
	//}

	t1.join();
}