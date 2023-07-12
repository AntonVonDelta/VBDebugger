#pragma once

#include "ExecutionController.h"
#include "Winsockets.h"

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
};