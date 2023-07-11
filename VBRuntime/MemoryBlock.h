#pragma once

#include <memory>

class MemoryBlock {
private:
	std::shared_ptr<char[]> data;
	uint32_t count = 0;

public:

	MemoryBlock(int size);

	// Do not allow this object to be copied
	MemoryBlock(const MemoryBlock& other);

	char* block();

	uint32_t size();
};

