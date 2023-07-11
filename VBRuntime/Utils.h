#pragma once

#include "Winsockets.h"
#include "MemoryBlock.h"
#include <memory>
#include <optional>

// 100MiB
#define MAX_PACKET_SIZE 104857600  

std::unique_ptr<MemoryBlock> readPacket(SOCKET socket);

template<typename T>
std::optional<T> readPacketModel(SOCKET socket);