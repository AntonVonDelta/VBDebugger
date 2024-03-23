#include "MemoryBlock.h"

MemoryBlock::MemoryBlock() {
	count = 0;
}

MemoryBlock::MemoryBlock(int size) {
	auto raw_data = new uint8_t[size];

	count = size;
	data = std::shared_ptr<uint8_t[]>(raw_data);
}

uint8_t* MemoryBlock::get() const {
	return data.get();
}

uint32_t MemoryBlock::size() const {
	return count;
}
