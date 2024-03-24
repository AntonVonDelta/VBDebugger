#pragma once

// This is set as a preprocessor directive
// So that on build time it's set, on use time it's missing
#ifdef LIBTASK_EXPORTS
#define DLL_API __declspec(dllexport)
#else
#define DLL_API __declspec(dllimport)
#endif
