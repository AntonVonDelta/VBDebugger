#include "NetModels.h"
#include "Util.h"
#include "flatbuffers/flatbuffers.h"

using namespace flatbuffers;

std::optional<MemoryBlock> NetModels::readPacket(SOCKET socket) {
	unsigned int packet_size;
	int read_bytes;
	MemoryBlock packet_data;

	read_bytes = recv(socket, (char*)&packet_size, 4, MSG_WAITALL);
	if (read_bytes == 0 || read_bytes == SOCKET_ERROR) return {};
	if (read_bytes > MAX_PACKET_SIZE) return {};

	// make_unique_for_overwrite does not value intialize the Ts
	// like make_unique does
	packet_data = MemoryBlock(packet_size);

	read_bytes = recv(socket, packet_data.get(), packet_size, MSG_WAITALL);
	if (read_bytes == 0) return {};

	return packet_data;
}

template<typename T>
std::optional<std::unique_ptr<T>> NetModels::readPacketModel(SOCKET socket) {
	flatbuffers::Verifier::Options verifier_options;
	bool verify_result;

	typedef T::TableType ApiObject;
	auto data = readPacket(socket);

	if (!data) return {};

	verifier_options.assert = true;

	flatbuffers::Verifier verifier((uint8_t*)data->get(), data->size(), verifier_options);
	const auto* root = flatbuffers::GetRoot<ApiObject>(data->get());
	verify_result = root->Verify(verifier);

	if (!verify_result) return {};

	return std::unique_ptr<T>(root->UnPack(nullptr));
}



bool NetModels::sendPacket(SOCKET socket, const char* data, uint32_t len) {
	uint32_t total_sent_bytes = 0;
	const char* buffer = data;

	do {
		int sent_bytes = send(socket, buffer, len - total_sent_bytes, 0);

		if (sent_bytes == SOCKET_ERROR) return false;

		buffer += sent_bytes;
		total_sent_bytes += sent_bytes;
	} while (total_sent_bytes != len);

	return true;
}

template<typename T>
bool NetModels::sendPacketModel(SOCKET socket, T& packet) {
	FlatBufferBuilder builder;
	typedef T::TableType ApiObject;
	MemoryBlock block;

	builder.FinishSizePrefixed(ApiObject::Pack(builder, &packet));

	return sendPacket(socket, (char*)builder.GetBufferPointer(), builder.GetSize());
}



// Register all models here so that the templates are compiled
// and ready to be linked when other units are compiled
template std::optional<std::unique_ptr<NetModels::DebuggerInfoT>> NetModels::readPacketModel(SOCKET socket);
template std::optional< std::unique_ptr<NetModels::DebugEventT>> NetModels::readPacketModel(SOCKET socket);
template std::optional< std::unique_ptr<NetModels::DebuggerAttachedT>> NetModels::readPacketModel(SOCKET socket);

template bool NetModels::sendPacketModel(SOCKET socket, NetModels::DebuggerAttachedT& packet);
