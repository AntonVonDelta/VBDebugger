#pragma once

#include "DebuggerServer.h"
#include "Debugger.h"
#include <string>
#include <map>
#include <vector>
#include <memory>
#include <functional>
#include <sstream>
#include <mutex>


class DebugSession {
private:
	struct Scope {
		std::string name;
		std::map<std::string, std::string> locals;
	};

	DebuggerServer debugger_server;
	std::unique_ptr<Debugger> current_debugger;
	std::mutex sync_current_debugger;
	std::vector<Scope> execution_stack;

	// Errors which are kept until the stack empties
	std::vector<std::string> error_messages;

public:
	DebugSession(int server_port);

	void traceEnterProcedure(std::string scope_name);
	void traceLeaveProcedure(std::string scope_name);

	void traceLog(int line_number, std::vector<std::string> arguments) {

	}

private:
	void addMessage(std::string message);

	void handleNewDebugger(std::unique_ptr<Debugger> debugger);
};

