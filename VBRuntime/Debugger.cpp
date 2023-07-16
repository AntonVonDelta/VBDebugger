#include "Debugger.h"
#include "NetModels.h"
#include <exception>

NetModels::StackDumpT generateStackDumpModel(ExecutionController* session);
template<typename T, typename U> std::unique_ptr<T> map(const U&) {
	static_assert(true, "A mapping was not defined");
	return nullptr;
}

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
					sendStackDump();
					break;

				case NetModels::CommandType::Resume:
					session->resume();
					break;

				case NetModels::CommandType::NextInstruction:
					session->stepOver(true);
					sendStackDump();
					break;

				case NetModels::CommandType::SkipInstruction:
					session->stepOver(false);
					sendStackDump();
					break;

				default:
					break;
			}
		}
	} catch (std::exception ex) {}

	// Resume execution after the debugger disconnects
	try {
		session->resume();
	} catch (std::exception ex) {}

	closeConnection();

	execution_detached = true;
}


bool Debugger::sendStackDump() {
	auto stack_dump_model = generateStackDumpModel(session);

	return sendPacketModel<NetModels::StackDumpT>(stack_dump_model);
}


template<typename T>
std::optional<std::unique_ptr<T>> Debugger::readPacketModel() {
	return NetModels::readPacketModel<T>(socket);
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



NetModels::StackDumpT generateStackDumpModel(ExecutionController* session) {
	NetModels::StackDumpT result;
	auto frames = session->getStack();
	auto current_instruction = session->getCurrentInstruction();
	auto message = session->getMessages();

	result.curent_instruction = map<NetModels::SourceCodeReferenceT>(current_instruction);

	for (const auto& stack_frame : frames) {
		std::unique_ptr<NetModels::StackFrameT> stack_frame_model = std::make_unique< NetModels::StackFrameT>();

		stack_frame_model->reference = map<NetModels::SourceCodeReferenceT>(stack_frame.scope_reference);

		for (const auto& local : stack_frame.locals) {
			std::unique_ptr<NetModels::VariableT> local_model = std::make_unique< NetModels::VariableT>();

			local_model->name = local.first;
			local_model->value = local.second;

			stack_frame_model->locals.push_back(std::move(local_model));
		}

		result.frames.push_back(std::move(stack_frame_model));
	}

	for (const auto& message : session->getMessages()) {
		result.messages.push_back(message);
	}

	return result;
}

std::unique_ptr<NetModels::SourceCodeReferenceT> map(const SourceCodeReference& reference) {
	std::unique_ptr<NetModels::SourceCodeReferenceT> result = std::make_unique< NetModels::SourceCodeReferenceT>();

	result->filename = reference.filename;
	result->scope_name = reference.scope_name;
	result->line_number = reference.line_number;

	return result;
}


