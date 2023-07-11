#include "Utils.h"
#include "flatbuffers/flatbuffers.h"
#include "DebugEvent_generated.h"
#include "DebuggerInfo_generated.h"

using namespace flatbuffers;

MemoryBlock readPacket(SOCKET socket) {
	unsigned int packet_size;
	int read_bytes;
	std::unique_ptr<MemoryBlock> packet_data;

	read_bytes = recv(socket, (char*)&packet_size, 4, MSG_WAITALL);
	if (read_bytes == 0) return nullptr;
	if (read_bytes > MAX_PACKET_SIZE) return nullptr;

	// make_unique_for_overwrite does not value intialize the Ts
	// like make_unique does
	packet_data = std::make_unique<MemoryBlock>(packet_size);

	read_bytes = recv(socket, packet_data.get()->block(), packet_size, MSG_WAITALL);
	if (read_bytes == 0) return nullptr;

	return packet_data;
}

template<typename T, typename U>
std::optional<T> readPacketModel(SOCKET socket) {
	Verifier verifier;
	Verifier::Options verifier_options;
	bool verifyResult;
	T* root;
	std::unique_ptr<char[]> data = readPacket(socket);

	if (!data) return {};


	verifier = Verifier(data.get(),data.);
	root = GetRoot<T>(data.get());
	verifyResult = root->Verify();

	if (!verifyResult) return {};
	return *root;
}

// Register all models here so that the templates are compiled
// and ready to be linked when other units are compiled
template std::optional<NetModels::DebuggerInfo> readPacketModel(SOCKET socket);
template std::optional<NetModels::DebugEvent> readPacketModel(SOCKET socket)