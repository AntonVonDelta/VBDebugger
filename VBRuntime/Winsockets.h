#pragma once

#define WIN32_LEAN_AND_MEAN // Stop the compiler from using "extra" definitions from windows.h which already includes some version of winsock

#include <Windows.h>
#include <Windows.h>
#include <WinSock2.h>
#include <WS2tcpip.h>

#define FOR_EVERY_CLIENT(var) if(pendingClients.size()) for (int var = pendingClients.size()-1;var>=0;var--)
