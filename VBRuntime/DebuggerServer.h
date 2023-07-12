#pragma once


#include "Debugger.h"
#include "NetModels.h"
#include "DebuggerInfo_generated.h"
#include <iostream>
#include <sstream>
#include <vector>
#include <map>
#include <string>
#include <thread>
#include <atomic>
#include <functional>
#include <memory>
#include <mutex>
#include <exception>
#include <optional>


class DebuggerServer {
private:
	struct CLIENT_STRUCTURE {
		SOCKET sockid;
		char address[INET6_ADDRSTRLEN];
		time_t connected_timestamp;
	};

	std::thread debugger_listener;
	std::atomic<bool> stop_debugging;
	std::function<void(std::string)> logger;
	bool server_started;

	int server_port;
	SOCKET listening_socket;
	fd_set master, read_fds;

#pragma region Events
	std::function<void(std::unique_ptr<Debugger>)> event_new_debugger;
#pragma endregion Events

public:
	DebuggerServer(int server_port);
	~DebuggerServer();

	void start();

	void setLogger(std::function<void(std::string)> handler) { logger = handler; }
	void registerNewDebugger(std::function <void(std::unique_ptr<Debugger>)> handler) { event_new_debugger = handler; }

private:
	void run();
	void processConnections();
	std::unique_ptr<Debugger> createDebugger(CLIENT_STRUCTURE protoClient);

	bool startServer();
	void stopServer();

	void log(std::string log);

	static void setRecvTimeout(SOCKET socket, bool enable);
	static void enableKeepAlive(SOCKET socket);

	void onNewDebugger(std::unique_ptr<Debugger> object) {
		if (event_new_debugger) event_new_debugger(std::move(object));
	}
};