#pragma once

#include "Task.h"
#include "Utils.h"

namespace TPL {
	std::unique_ptr<TPL::Task<void>> DLL_API WhenAny(std::vector<std::shared_ptr<TPL::BaseTask>> tasks);
	std::unique_ptr<TPL::Task<void>> DLL_API WhenAll(std::vector<std::shared_ptr<TPL::BaseTask>> tasks);
}
