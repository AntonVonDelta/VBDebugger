#include "Debugger.h"

Debugger::Debugger(SOCKET socket) {
	this->socket = socket;
}

Debugger::~Debugger() {
	closeConnection();
}

void Debugger::attachDebugger(ExecutionController* session) {
	// sendDebuggerAttached

	while (true) {
		// read debug event

		// for types pause, nextinstruction, skip instruction, send back the entire stack
	}

	// resume execution because the debugger has detached

}
