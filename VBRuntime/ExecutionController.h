#pragma once

#include "Breakpoint.h"
#include <string>
#include <map>
#include <vector>
#include <memory>
#include <functional>
#include <sstream>
#include <mutex>
#include <sstream>

struct SourceCodeReference {
	std::string filename;
	std::string scope_name;
	int line_number;

	std::string toString() {
		std::ostringstream stream;

		stream << "{";
		stream << "\"filename\":\"" << filename << "\"" << ", ";
		stream << "\"scope_name\":\"" << scope_name << "\"" << ", ";
		stream << "\"line_number\":\"" << line_number << "\"";
		stream << "}";

		return stream.str();
	}
};

struct Scope {
	SourceCodeReference scope_reference;
	SourceCodeReference current_instruction;

	std::map<std::string, std::string> locals;
};

class ExecutionController {
private:
	std::vector<Scope> execution_stack;

	// Errors which are kept until the stack empties
	std::vector<std::string> error_messages;

	Breakpoint breakpoint = {};
public:

	void pause();
	void resume();
	void stepOver(bool execute_instruction);

	std::vector<Scope> getStack();
	std::vector<std::string> getMessages();

	void traceEnterProcedure(SourceCodeReference& reference, std::vector<std::string> arguments);
	bool traceLog(SourceCodeReference& reference, std::vector<std::string> arguments);
	void traceLeaveProcedure(SourceCodeReference& reference, std::vector<std::string> arguments);

private:
	void addMessage(std::string message);
	void checkReferenceWithLiveScope(SourceCodeReference& reference);
	void addLocalsToLiveScope(SourceCodeReference& reference, std::vector<std::string>& arguments);

	bool isReferencePartOfScope(const Scope& scope, const SourceCodeReference& reference);

	/// <summary>
	/// Pops the current stack frame and tries to fix any mismatches
	/// with the current instruction by finding the closest resembling frame.
	/// </summary>
	void syncAndPopStack(SourceCodeReference& reference);
};

