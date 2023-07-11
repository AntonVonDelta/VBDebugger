#include "MessageBroker.h"
#include <iostream>
#include <thread>
#include <chrono>

using namespace std;

enum Message {
	Test1,
	Hello
};

MessageBroker<Message> broker;

void run() {

	cout << "waiting for message" << endl;

	Message message;
	auto response = broker.getMessage(message, true);

	if (response) {
		cout << "message: " << message << endl;
		cout << "setting the promise soon" << endl;
		std::this_thread::sleep_for(std::chrono::seconds(5));

		response.value().set_value({});
	} else {
		cout << " no message" << endl;
	}

	std::this_thread::sleep_for(std::chrono::seconds(10));
}

int main() {
	auto t1 = thread(&run);

	std::this_thread::sleep_for(std::chrono::seconds(5));

	cout << "sending message" << endl;
	auto task = broker.sendMessage(Message::Hello);

	cout << "waiting on task to finalize" << endl;
	auto result = task.get();
	cout << "task finished, response has data " << (bool)result << endl;

	t1.join();
}