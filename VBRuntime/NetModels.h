#pragma once

#include "Winsockets.h"
#include "MemoryBlock.h"
#include "BreakpointEvent_generated.h"
#include "DebuggerInfo_generated.h"
#include "DebuggerAttached_generated.h"
#include "DebugCommand_generated.h"
#include "StackDump_generated.h"
#include <memory>
#include <optional>

// 100MiB
#define MAX_PACKET_SIZE 104857600  

namespace NetModels {
	std::optional<MemoryBlock> readPacket(SOCKET socket);

	template<typename T>
	std::optional<std::unique_ptr<T>> readPacketModel(SOCKET socket);


	bool sendPacket(SOCKET socket, const char* data, uint32_t len);

	template<typename T>
	bool sendPacketModel(SOCKET socket, T& packet);
};