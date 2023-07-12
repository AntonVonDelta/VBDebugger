#include "NetModels.h"
#include "flatbuffers/flatbuffers.h"
#include "DebugEvent_generated.h"
#include "DebuggerInfo_generated.h"

using namespace flatbuffers;

std::optional<MemoryBlock> readPacket(SOCKET socket) {
	unsigned int packet_size;
	int read_bytes;
	MemoryBlock packet_data;

	read_bytes = recv(socket, (char*)&packet_size, 4, MSG_WAITALL);
	if (read_bytes == 0) return {};
	if (read_bytes > MAX_PACKET_SIZE) return {};

	// make_unique_for_overwrite does not value intialize the Ts
	// like make_unique does
	packet_data = MemoryBlock(packet_size);

	read_bytes = recv(socket, packet_data.get(), packet_size, MSG_WAITALL);
	if (read_bytes == 0) return {};

	return packet_data;
}

template<typename T>
std::optional<std::unique_ptr<T>> readPacketModel(SOCKET socket) {
	Verifier verifier;
	Verifier::Options verifier_options;
	bool verifyResult;
	T::TableType* root;
	auto data = readPacket(socket);

	if (!data) return {};

	verifier = Verifier(data->get(), data->count, verifier_options);
	root = GetRoot<T::TableType>(data->get());
	verifyResult = root->Verify();

	if (!verifyResult) return {};

	return std::unique_ptr <T>(root->UnPack(nullptr));
}

// Register all models here so that the templates are compiled
// and ready to be linked when other units are compiled
template std::optional<NetModels::DebuggerInfoT> readPacketModel(SOCKET socket);
template std::optional<NetModels::DebugEventT> readPacketModel(SOCKET socket)