#include "MemoryBlock.h"

MemoryBlock::MemoryBlock(int size) {
	data = std::make_shared_for_overwrite<char[]>(size);
}
MemoryBlock::MemoryBlock(const MemoryBlock& other) {
	data=std::move()
}

char* MemoryBlock::block() {
	return data.get();
}
char* MemoryBlock::get() {
	return nullptr;
}

uint32_t MemoryBlock::size() {
	return count;
}
