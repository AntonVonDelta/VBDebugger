#include "Debugger.h"

Debugger::Debugger(SOCKET socket) {
	this->socket = socket;
}

Debugger::Debugger(Debugger&& other) {
	socket = other.socket;
	socket_closed = other.socket_closed;

	other.socket_closed = true;
}

Debugger& Debugger::operator=(Debugger&& other) {
	socket = other.socket;
	socket_closed = other.socket_closed;

	other.socket_closed = true;

	return *this;
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

void Debugger::closeConnection() {
	if (socket_closed) return;
	socket_closed = true;

	shutdown(socket, SD_SEND);
	closesocket(socket);
}