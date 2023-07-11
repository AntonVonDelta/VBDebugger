#include "DebugSession.h"

DebugSession::DebugSession(int server_port) : debugger_server(server_port) {
	debugger_server.registerNewDebugger([this](auto debugger) {
		handleNewDebugger(std::move(debugger));
		});

	debugger_server.start();
}

void DebugSession::traceEnterProcedure(std::string scope_name) {
	Scope scope = { .name = scope_name };

	execution_stack.push_back(scope);
}

void DebugSession::traceLeaveProcedure(std::string scope_name) {
	auto scope = execution_stack.front();

	if (scope.name != scope_name) {
		std::ostringstream message;

		message << "Name of exiting scope mismatch with registered scope ";
		message << scope_name << " vs " << scope.name;
		addMessage(message.str());
	}
}

void DebugSession::addMessage(std::string message) {
	error_messages.push_back(message);
}

void DebugSession::handleNewDebugger(std::unique_ptr<Debugger> debugger) {
	std::lock_guard<std::mutex> lock(sync_current_debugger);

	if (!current_debugger) {
		current_debugger = std::move(debugger);

		return;
	}

	// Detect if current debugger is dead
	// and update with this fresh copy

	// else close this debugger
	debugger->close();
}
