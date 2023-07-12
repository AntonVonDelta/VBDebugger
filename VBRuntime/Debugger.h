#pragma once

#include "ExecutionController.h"
#include "Winsockets.h"

class Debugger {
private:
	SOCKET socket;
	bool socket_closed = false;

public:
	Debugger(SOCKET socket);
	Debugger(const Debugger&) = delete;
	Debugger& operator=(const Debugger&) = delete;

	Debugger(Debugger&& other);
	Debugger& operator=(Debugger&& other);

	~Debugger();

	void attachDebugger(ExecutionController* session);

private:
	void closeConnection();
};