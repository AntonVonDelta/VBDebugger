#include "ExecutionController.h"

void ExecutionController::pause() {
	breakpoint.pause();
}
void ExecutionController::resume() {
	breakpoint.resume();
}
void ExecutionController::stepOver(bool execute_instruction) {
	breakpoint.stepOver(execute_instruction);
}

std::vector<Scope> ExecutionController::getStack() {
	return execution_stack;
}
std::vector<std::string> ExecutionController::getMessages() {
	return error_messages;
}

void ExecutionController::traceEnterProcedure(SourceCodeReference& reference, std::vector<std::string> arguments) {
	Scope scope = { reference,{} };

	execution_stack.push_back(scope);

	breakpoint.input();
}

void ExecutionController::traceLeaveProcedure(SourceCodeReference& reference) {
	if (execution_stack.size() == 0) {
		std::ostringstream message;

		message << "Exiting scope but no stack frames available: " << reference.toString();

		addMessage(message.str());

		breakpoint.input();
	} else {
		checkReferenceWithLiveScope(reference);

		breakpoint.input();

		// Remove this scope after the breakpoint is signaled
		execution_stack.pop_back();
	}

	if (execution_stack.size() == 0) {
		// Clean message
		error_messages.clear();
	}
}

bool ExecutionController::traceLog(SourceCodeReference& reference, std::vector<std::string> arguments) {
	if (execution_stack.size() == 0) {
		std::ostringstream message;

		message << "No current scope for current instruction: " << reference.toString();

		addMessage(message.str());
	} else {
		auto last_scope = execution_stack.front();

		checkReferenceWithLiveScope(reference);

		addLocalsToLiveScope(arguments);
	}

	return breakpoint.input();
}

void ExecutionController::addMessage(std::string message) {
	error_messages.push_back(message);
}

void ExecutionController::checkReferenceWithLiveScope(SourceCodeReference& reference) {
	auto last_scope = execution_stack.front();
	auto last_scope_name = last_scope.reference.scope_name;
	auto current_scope_name = reference.scope_name;

	if (last_scope_name != current_scope_name) {
		std::ostringstream message;

		message << "Name of exiting scope mismatch with registered scope: ";
		message << reference.toString() << ", " << current_scope_name << ", " << last_scope_name << ", ";

		addMessage(message.str());
	}
}

void ExecutionController::addLocalsToLiveScope(std::vector<std::string>& arguments) {
	auto& last_scope = execution_stack.front();

	if (arguments.size() == 0) return;

	if (arguments.size() % 2 != 0) {
		std::ostringstream message;

		message << "Incorrect number of arguments passed to breakpoint: ";
		message << last_scope.reference.toString() << ", ";

		for (const auto& el : arguments) {
			message << el << ", ";
		}

		addMessage(message.str());
	}

	for (int i = 0; i < arguments.size() - 1; i += 1) {
		last_scope.locals[arguments[i]] = arguments[i + 1];
	}
}
