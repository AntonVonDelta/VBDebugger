#include "Winsockets.h"
#include "ExecutionController.h"
#include "DebuggerServer.h"
#include <iostream>


using namespace std;

DebuggerServer debugger_server(5050);
ExecutionController execution_controller;

void WINAPI Init() {
	OutputDebugStringA("Init");

	debugger_server.start();
}

void WINAPI Log(char* filename, char* scope_name, int line_number, char* arguments) {
	SourceCodeReference reference;

	reference.filename = filename;
	reference.scope_name = scope_name;
	reference.line_number = line_number;

	OutputDebugStringA("Log");

	execution_controller.traceLog(reference, { "var",arguments });
}

void WINAPI EnterProcedure(char* filename, char* scope_name, int line_number, char* arguments) {
	SourceCodeReference reference;

	reference.filename = filename;
	reference.scope_name = scope_name;
	reference.line_number = line_number;

	OutputDebugStringA("EnterProcedure");

	execution_controller.traceEnterProcedure(reference, { "var",arguments });
}

void WINAPI LeaveProcedure(char* filename, char* scope_name, int line_number) {
	SourceCodeReference reference;

	reference.filename = filename;
	reference.scope_name = scope_name;
	reference.line_number = line_number;

	OutputDebugStringA("LeaveProcedure");

	execution_controller.traceLeaveProcedure(reference);
}
