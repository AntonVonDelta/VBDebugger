#pragma once

#include "Task.h"

std::unique_ptr<TPL::Task> WhenAny(std::vector<std::shared_ptr<TPL::Task>> tasks);