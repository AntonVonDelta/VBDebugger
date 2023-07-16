#include "ExecutionController.h"
#include "DebuggerServer.h"
#include "Debugger.h"
#include <iostream>
#include <memory>
#include <functional>

using namespace std;

DebuggerServer debugger_server(5050);
ExecutionController execution_controller;
std::unique_ptr<Debugger> debugger;

void WINAPI Init() {
	OutputDebugStringA("Init\n");

	debugger_server.registerNewDebugger([](std::unique_ptr<Debugger> temp_debugger) {
		if (debugger && !debugger->isDetached()) return;
		debugger = std::move(temp_debugger);

		debugger->attachDebugger(&execution_controller);
		});

	debugger_server.start();
}

void WINAPI EnterProcedure(const char* filename, const char* scope_name, int line_number, const char* arguments) {
	SourceCodeReference reference;

	reference.filename = filename;
	reference.scope_name = scope_name;
	reference.line_number = line_number;

	OutputDebugStringA("EnterProcedure\n");

	execution_controller.traceEnterProcedure(reference, { "var", arguments });
}

void WINAPI Log(const char* filename, const char* scope_name, int line_number, const char* arguments) {
	SourceCodeReference reference;

	reference.filename = filename;
	reference.scope_name = scope_name;
	reference.line_number = line_number;

	//OutputDebugStringA("Log\n");

	execution_controller.traceLog(reference, { "var", arguments });
}

void WINAPI LeaveProcedure(const char* filename, const char* scope_name, int line_number) {
	SourceCodeReference reference;

	reference.filename = filename;
	reference.scope_name = scope_name;
	reference.line_number = line_number;

	OutputDebugStringA("LeaveProcedure\n");

	execution_controller.traceLeaveProcedure(reference);
}
