#pragma once

#include <memory>

class MemoryBlock {
private:
	std::shared_ptr<char[]> data;
	uint32_t count = 0;

public:
	MemoryBlock();
	MemoryBlock(int size);

	char* get() const;

	uint32_t size() const;
};

