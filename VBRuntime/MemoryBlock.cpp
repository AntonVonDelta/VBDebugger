#include "MemoryBlock.h"

MemoryBlock::MemoryBlock() {
	count = 0;
}

MemoryBlock::MemoryBlock(int size) {
	auto raw_data = new char[size];

	count = size;
	data = std::shared_ptr<char[]>(raw_data);
}

char* MemoryBlock::get() const {
	return data.get();
}

uint32_t MemoryBlock::size() const {
	return count;
}
