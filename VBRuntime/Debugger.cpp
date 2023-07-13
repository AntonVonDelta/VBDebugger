#include "Debugger.h"
#include "NetModels.h"

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


template<typename T>
std::optional<std::unique_ptr<T>> Debugger::readPacketModel() {
	return NetModels::readPacketModel(socket);
}

template<typename T>
bool Debugger::sendPacketModel(T& packet) {
	return false;
}


void Debugger::closeConnection() {
	if (socket_closed) return;
	socket_closed = true;

	shutdown(socket, SD_SEND);
	closesocket(socket);
}