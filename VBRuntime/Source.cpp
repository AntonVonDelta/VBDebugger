#include "ExecutionController.h"
#include "DebuggerServer.h"
#include "Debugger.h"
#include <iostream>
#include <memory>
#include <functional>
#include <string>
#include <vector>
#include <regex>

using namespace std;

std::vector<std::string> deserializeArguments(std::string data);


HINSTANCE dllInstance;
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

	execution_controller.traceEnterProcedure(reference, deserializeArguments(arguments));
}

void WINAPI Log(const char* filename, const char* scope_name, int line_number, const char* arguments) {
	SourceCodeReference reference;

	reference.filename = filename;
	reference.scope_name = scope_name;
	reference.line_number = line_number;

	//OutputDebugStringA("Log\n");

	execution_controller.traceLog(reference, deserializeArguments(arguments));
}

void WINAPI LeaveProcedure(const char* filename, const char* scope_name, int line_number) {
	SourceCodeReference reference;

	reference.filename = filename;
	reference.scope_name = scope_name;
	reference.line_number = line_number;

	OutputDebugStringA("LeaveProcedure\n");

	execution_controller.traceLeaveProcedure(reference);
}

std::vector<std::string> deserializeArguments(std::string data) {
	std::vector<std::string> result;
	std::string temp_value = "";
	bool previousCharWasEscape = false;

	for (const auto& el : data) {
		// Use flag to escape forward slashes
		if (el == '\\') {
			if (previousCharWasEscape) {
				// Previous slash escaped this one
				result.push_back(temp_value);
			}

			previousCharWasEscape = !previousCharWasEscape;
		} else if (el == ',' && !previousCharWasEscape) {
			result.push_back(temp_value);
			temp_value.clear();
		} else {
			temp_value += el;
		}
	}

	if(temp_value.size()!=0)
		result.push_back(temp_value);

	return result;
}



BOOL WINAPI DllMain(
	HINSTANCE hinstDLL,  // handle to DLL module
	DWORD fdwReason,     // reason for calling function
	LPVOID lpvReserved)  // reserved
{
	dllInstance = hinstDLL;

	switch (fdwReason) {
		case DLL_PROCESS_ATTACH:
			// Initialize once for each new process.
			// Return FALSE to fail DLL load.
			OutputDebugStringA("DLL_PROCESS_ATTACH");

			break;

		case DLL_THREAD_ATTACH:
			// Do thread-specific initialization.
			OutputDebugStringA("DLL_THREAD_ATTACH");

			break;

		case DLL_THREAD_DETACH:
			// Do thread-specific cleanup.
			OutputDebugStringA("DLL_THREAD_DETACH");

			break;

		case DLL_PROCESS_DETACH:

			if (lpvReserved != nullptr) {
				OutputDebugStringA("DLL_PROCESS_DETACH termination");

				break; // do not do cleanup if process termination scenario
			}

			OutputDebugStringA("DLL_PROCESS_DETACH normal");

			// Perform any necessary cleanup.
			break;
	}
	return TRUE;  // Successful DLL_PROCESS_ATTACH.
}