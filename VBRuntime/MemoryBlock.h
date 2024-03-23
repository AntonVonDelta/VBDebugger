#pragma once

#include <memory>

class MemoryBlock {
private:
	std::shared_ptr<uint8_t[]> data;
	uint32_t count = 0;

public:
	MemoryBlock();
	MemoryBlock(int size);

	uint8_t* get() const;
	uint32_t size() const;
};

