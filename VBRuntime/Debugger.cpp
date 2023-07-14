#include "Debugger.h"
#include "NetModels.h"
#include <exception>

Debugger::Debugger(SOCKET socket) {
	this->socket = socket;
}
Debugger::~Debugger() {
	if (processing_thread.joinable()) processing_thread.join();

	closeConnection();
}


bool Debugger::isDetached() {
	return execution_detached;
}

void Debugger::attachDebugger(ExecutionController* session) {
	this->session = session;

	processing_thread = std::thread(&Debugger::loop, this);
}


void Debugger::loop() {
	NetModels::DebuggerAttachedT debugger_attached;

	sendPacketModel(debugger_attached);

	try {
		while (true) {
			auto command = readPacketModel<NetModels::DebugCommandT>();

			if (!command) break;

			switch (command->get()->command_type) {
				case NetModels::CommandType::Pause:
					session->pause();
					break;

				case NetModels::CommandType::Resume:
					session->resume();
					break;

				case NetModels::CommandType::NextInstruction:
					session->stepOver(true);
					break;

				case NetModels::CommandType::SkipInstruction:
					session->stepOver(false);
					break;

				default:
					break;
			}
		}
	} catch (std::exception ex) {}

	// resume execution because the debugger has detached

	closeConnection();

	execution_detached = true;
}



bool Debugger::sendStackDump() {
	NetModels::StackDumpT dump;

	auto stacks = session->getStack();
	auto message = session->getMessages();

	for (const auto& stack_frame : stacks) {
		std::unique_ptr<NetModels::StackFrameT> stack_frame_model = std::make_unique< NetModels::StackFrameT>();

		stack_frame_model->filename = stack_frame.reference.filename;
		stack_frame_model->scope_name = stack_frame.reference.scope_name;
		stack_frame_model->line_number = stack_frame.reference.line_number;

		for (const auto& local : stack_frame.locals) {
			std::unique_ptr<NetModels::VariableT> local_model = std::make_unique< NetModels::VariableT>();

			local_model->name = local.first;
			local_model->value = local.second;

			stack_frame_model->locals.push_back(local_model);
		}

		dump.frames.push_back(stack_frame_model);
	}

	for (const auto& message : session->getMessages()) {
		dump.messages.push_back(message);
	}

	return sendPacketModel<NetModels::StackDumpT>(dump);
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