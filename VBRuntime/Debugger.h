#pragma once

#include "ExecutionController.h"
#include "Winsockets.h"
#include <optional>
#include <memory>

class Debugger {
private:
	SOCKET socket;
	bool socket_closed = false;

public:
	Debugger(SOCKET socket);
	~Debugger();

	void attachDebugger(ExecutionController* session);

private:
	void closeConnection();

	template<typename T>
	std::optional<std::unique_ptr<T>> readPacketModel();

	template<typename T>
	bool sendPacketModel(T& packet);
};
