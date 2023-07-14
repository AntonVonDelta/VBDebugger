#pragma once

#include "ExecutionController.h"
#include "Winsockets.h"
#include "NetModels.h"
#include <optional>
#include <memory>
#include <atomic>
#include <thread>

class Debugger {
private:
	SOCKET socket;
	ExecutionController* session = nullptr;
	std::thread processing_thread;
	std::atomic<bool> execution_detached = false;
	bool socket_closed = false;

public:
	Debugger(SOCKET socket);
	~Debugger();

	bool isDetached();
	void attachDebugger(ExecutionController* session);

private:
	void loop();

	bool sendStackDump();

	template<typename T>
	std::optional<std::unique_ptr<T>> readPacketModel();

	template<typename T>
	bool sendPacketModel(T& packet);

	void closeConnection();
};
