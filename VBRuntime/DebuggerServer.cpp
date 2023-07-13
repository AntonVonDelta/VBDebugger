#include "DebuggerServer.h"
#include "NetModels.h"

DebuggerServer::DebuggerServer(int server_port) {
	this->server_port = server_port;

	logger = [](std::string log) {
		OutputDebugStringA(log.c_str());
	};
}
DebuggerServer::~DebuggerServer() {
	stop_debugging = true;

	if (debugger_listener.joinable())
		debugger_listener.join();

	if (server_started)
		stopServer();
}

void DebuggerServer::start() {
	debugger_listener = std::thread(&DebuggerServer::run, this);
}

void DebuggerServer::run() {
	if (!startServer()) return;

	server_started = true;
	processConnections();
}
void DebuggerServer::processConnections() {
	sockaddr_in clientinfo;
	char strAddr[INET6_ADDRSTRLEN];
	int selRes;
	time_t current_time;
	SOCKET client;
	timeval timeout = { 0 , 500000 };

	FD_ZERO(&master);
	FD_ZERO(&read_fds);
	FD_SET(listening_socket, &master);

	// Listen for debuggers
	while (!stop_debugging) {
		read_fds = master;

		// Check client validity after 5 sec. timeout
		time(&current_time);

		// Get new connections - check if server socket contains "connections" to be read
		if ((selRes = select(listening_socket + 1, &read_fds, NULL, NULL, &timeout)) < 1) continue;

		if (FD_ISSET(listening_socket, &read_fds)) {
			int sz = sizeof(clientinfo);

			client = accept(listening_socket, (sockaddr*)&clientinfo, &sz);

			if (client == SOCKET_ERROR)	log("One accept() failed !");
			else {
				std::ostringstream string_builder;
				CLIENT_STRUCTURE protoClient;

				// Measure the time the socket connected
				time(&protoClient.connected_timestamp);

				memset(&protoClient, 0, sizeof(protoClient));
				protoClient.sockid = client;
				strcpy_s(protoClient.address, INET6_ADDRSTRLEN, InetNtopA(clientinfo.sin_family, &(clientinfo.sin_addr), strAddr, INET6_ADDRSTRLEN));

				// Set timeout for recv command - or else it will block
				setRecvTimeout(protoClient.sockid, true);

				string_builder << "New connection to server from " << protoClient.address;
				log(string_builder.str().c_str());

				// Create debugger instance
				auto debugger = createDebugger(protoClient);
				onNewDebugger(std::move(debugger));
			}
		}
	}
}
std::unique_ptr<Debugger> DebuggerServer::createDebugger(CLIENT_STRUCTURE protoClient) {
	auto init_packet =NetModels::readPacketModel<NetModels::DebuggerInfoT>(protoClient.sockid);

	if (!init_packet) {
		log("Failed init packet");

		closesocket(protoClient.sockid);
		return nullptr;
	}

	auto model = init_packet->get();

	log(std::string("Debugger connected: ") + model->name);

	try {
		setRecvTimeout(protoClient.sockid, false);
		enableKeepAlive(protoClient.sockid);
	} catch (std::exception ex) {
		return nullptr;
	}

	return std::make_unique<Debugger>(protoClient.sockid);
}


bool DebuggerServer::startServer() {
	SOCKADDR_IN server_config;
	WSADATA wsaData;

	// Init sockets
	if (WSAStartup(MAKEWORD(2, 0), &wsaData)) {
		log("WSAStartup failed! Could not initialize sockets!");
		return false;
	}

	// Create the listening server socket
	listening_socket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (listening_socket == INVALID_SOCKET) {
		log("Error opening the socket!");
		return false;
	}

	// Bind to the listening address
	server_config.sin_family = AF_INET;
	server_config.sin_port = htons(server_port);
	server_config.sin_addr.s_addr = htonl(INADDR_ANY);
	memset(server_config.sin_zero, 0, sizeof(server_config.sin_zero));


	if (bind(listening_socket, (SOCKADDR*)&server_config, sizeof(server_config)) == SOCKET_ERROR) {
		log("Error in binding socket to port");
		closesocket(listening_socket);
		WSACleanup();	// We better cleanup ourselves: it's possible the port will get locked next time we run (cause timeout)
		return false;
	}

	if (listen(listening_socket, SOMAXCONN) == SOCKET_ERROR) {
		log("Error in listening!");
		closesocket(listening_socket);
		WSACleanup();
		return false;
	}

	return true;
}
void DebuggerServer::stopServer() {
	closesocket(listening_socket);
	WSACleanup();
}

void DebuggerServer::setRecvTimeout(SOCKET socket, bool enable) {
	DWORD maxWaitTime = enable ? 5000 : 0;

	// Disable rcv timeout
	setsockopt(socket, SOL_SOCKET, SO_RCVTIMEO, (char*)&maxWaitTime, sizeof(maxWaitTime));
}

void DebuggerServer::enableKeepAlive(SOCKET socket) {

	// Enable keep alive
	int mode = 1;	//<>0 enable

	if (setsockopt(socket, SOL_SOCKET, SO_KEEPALIVE, (char*)&mode, sizeof(int)) == SOCKET_ERROR) {
		//This feature is very important. If not active then no connection
		throw std::runtime_error("No keepalive supported");
	}

	//Set first the keep alive characetrsitc...no more timeouts messing
	tcp_keepalive params;
	params.keepalivetime = 5000;
	params.keepaliveinterval = 5000;
	params.onoff = 1;

	DWORD output = 0;
	if (WSAIoctl(socket, SIO_KEEPALIVE_VALS, &params, sizeof(tcp_keepalive), 0, 0, &output, 0, 0) != 0) {
		//This feature is very important. If not active then no connection
		throw std::runtime_error("No keepalive supported");
	}
}

void DebuggerServer::log(std::string log) {
	logger(log);
}
