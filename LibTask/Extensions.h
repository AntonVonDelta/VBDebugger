#pragma once

#include "Task.h"
#include "Utils.h"

namespace TPL {
	std::unique_ptr<TPL::Task> DLL_API WhenAny(std::vector<std::shared_ptr<TPL::Task>> tasks);
	std::unique_ptr<TPL::Task> DLL_API WhenAll(std::vector<std::shared_ptr<TPL::Task>> tasks);
}
