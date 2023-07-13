#include "Debugger.h"
#include "NetModels.h"

Debugger::Debugger(SOCKET socket) {
	this->socket = socket;
}
Debugger::~Debugger() {
	closeConnection();
}


bool Debugger::isDetached() {
	return execution_detached;
}

void Debugger::attachDebugger(ExecutionController* session) {
	NetModels::DebuggerAttachedT debugger_attached;

	sendPacketModel(debugger_attached);
	// sendDebuggerAttached

	//while (true) {
		// read debug event

		// for types pause, nextinstruction, skip instruction, send back the entire stack
	//}

	// resume execution because the debugger has detached

	closeConnection();

	execution_detached = true;
}




template<typename T>
std::optional<std::unique_ptr<T>> Debugger::readPacketModel() {
	return NetModels::readPacketModel(socket);
}

template<typename T>
bool Debugger::sendPacketModel(T& packet) {
	return NetModels::sendPacketModel(socket, packet);
}

void Debugger::closeConnection() {
	if (socket_closed) return;
	socket_closed = true;

	closesocket(socket);
}