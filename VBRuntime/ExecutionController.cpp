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
SourceCodeReference ExecutionController::getCurrentInstruction() {
	return current_instruction;
}
std::vector<std::string> ExecutionController::getMessages() {
	return error_messages;
}

void ExecutionController::traceEnterProcedure(SourceCodeReference& reference, std::vector<std::string> arguments) {
	Scope scope;

	current_instruction = reference;

	scope.scope_reference = reference;
	scope.locals = {};

	execution_stack.push_back(scope);
	addLocalsToLiveScope(reference, arguments);

	breakpoint.input();
}
bool ExecutionController::traceLog(SourceCodeReference& reference, std::vector<std::string> arguments) {
	current_instruction = reference;

	if (execution_stack.size() == 0) {
		std::ostringstream message;

		message << "No current scope for current instruction: " << reference.toString();

		addMessage(message.str());
	} else {
		auto last_scope = execution_stack.back();

		checkReferenceWithLiveScope(reference);
		addLocalsToLiveScope(current_instruction, arguments);
	}

	return breakpoint.input();
}
void ExecutionController::traceLeaveProcedure(SourceCodeReference& reference, std::vector<std::string> arguments) {
	current_instruction = reference;

	if (execution_stack.size() == 0) {
		std::ostringstream message;

		message << "Exiting scope but no stack frames available: " << reference.toString();

		addMessage(message.str());

		breakpoint.input();
	} else {
		checkReferenceWithLiveScope(reference);
		addLocalsToLiveScope(current_instruction, arguments);

		breakpoint.input();

		// Remove this scope after the breakpoint is signaled
		syncAndPopStack(reference);
	}

	if (execution_stack.size() == 0) {
		// Clean message
		error_messages.clear();
	}
}



void ExecutionController::addMessage(std::string message) {
	error_messages.push_back(message);
}

void ExecutionController::checkReferenceWithLiveScope(SourceCodeReference& reference) {
	auto current_scope = execution_stack.back();
	auto current_scope_name = current_scope.scope_reference.scope_name;
	auto instruction_scope_name = reference.scope_name;

	if (!isReferencePartOfScope(current_scope, reference)) {
		std::ostringstream message;

		message << "Current scope mismatch with current instruction's scope: ";
		message << reference.toString() << ", " << instruction_scope_name << ", " << current_scope_name << ", ";

		addMessage(message.str());
	}
}

void ExecutionController::addLocalsToLiveScope(SourceCodeReference& reference, std::vector<std::string>& arguments) {
	auto& current_scope = execution_stack.back();

	if (arguments.size() == 0) return;

	if (arguments.size() % 2 != 0) {
		std::ostringstream message;

		message << "Incorrect number of arguments passed to breakpoint: ";
		message << reference.toString() << ", ";

		for (const auto& el : arguments) {
			message << el << ", ";
		}

		addMessage(message.str());

		// We can't add any variable because we don't have pairs
		return;
	}

	for (int i = 0; i < arguments.size() - 1; i += 2) {
		current_scope.locals[arguments[i]] = arguments[i + 1];
	}
}

bool ExecutionController::isReferencePartOfScope(const Scope& scope, const SourceCodeReference& reference) {
	auto current_scope = execution_stack.back();
	auto current_scope_name = current_scope.scope_reference.scope_name;
	auto instruction_scope_name = reference.scope_name;

	return current_scope_name == instruction_scope_name;
}

void ExecutionController::syncAndPopStack(SourceCodeReference& reference) {
	auto current_scope = execution_stack.back();
	int i;

	// Search for possible matching scope
	// and pop until reach that one
	// Otherwise just pop the current one

	for (i = execution_stack.size() - 1; i >= 0; i--) {
		const auto& scope = execution_stack[i];

		if (isReferencePartOfScope(scope, reference)) break;
	}

	if (i == -1) {
		// No stack found
		execution_stack.pop_back();
	} else {
		execution_stack.erase(execution_stack.begin() + i, execution_stack.end());
	}
}
