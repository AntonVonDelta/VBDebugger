#include "Winsockets.h"
#include "DebugSession.h"
#include <iostream>


using namespace std;

DebugSession debug_session(5050);

void WINAPI Init() {
	OutputDebugStringA("Init");
}

void WINAPI Log(int line_number) {
	OutputDebugStringA("Log");

	debug_session.traceLog(line_number, {});
}

void WINAPI EnterProcedure(char* scope_name) {
	OutputDebugStringA("EnterProcedure");

	debug_session.traceEnterProcedure(scope_name);
}

void WINAPI LeaveProcedure(char* scope_name) {
	OutputDebugStringA("LeaveProcedure");

	debug_session.traceLeaveProcedure(scope_name);
}
