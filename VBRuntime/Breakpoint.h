#pragma once

#include <mutex>
#include <condition_variable>
#include <memory>
#include <atomic>
#include <exception>

class Breakpoint {
private:
	enum class RunMode {
		FreeRunning,

		// This tells the input to switch to a slower
		// mutex and waiting for the debugger to signal
		Step
	};
	enum ExecutionState {
		Running,
		Stopped
	};

	std::atomic<RunMode> run_mode = RunMode::FreeRunning;
	std::mutex mtx_source_signal = {};
	std::condition_variable source_signal = {};
	std::mutex mtx_debugger_signal = {};
	std::condition_variable debugger_signal = {};
	ExecutionState execution_state;

public:

	// Called by the execution side
	void input() {
		if (run_mode == RunMode::FreeRunning) return;

		// Prepare lock to sync execution_state for 
		// source signal to notify others
		std::unique_lock<std::mutex> lock_source(mtx_source_signal);

		// Prepare lock in order to prevent others from signaling this 
		// execution to continue without us being prepared to listen for the
		// signal
		std::unique_lock<std::mutex> lock_debugger(mtx_debugger_signal);

		execution_state = ExecutionState::Stopped;
		lock_source.unlock();

		source_signal.notify_all();

		debugger_signal.wait(lock_debugger, [this]() {return execution_state == ExecutionState::Running; });
	}


	// Called by the right controlling side
	void pause() {
		if (run_mode != RunMode::FreeRunning)
			throw std::runtime_error("The breakpoint is already in step mode");

		run_mode = RunMode::Step;

		std::unique_lock<std::mutex> lock(mtx_source_signal);

		source_signal.wait(lock, [this]() {return execution_state == ExecutionState::Stopped; });
	}

	// Continue processing without breakpoints
	void resume() {
		if (run_mode != RunMode::Step)
			throw std::runtime_error("Cannot resume execution because it is already running");

		{
			std::unique_lock<std::mutex> lock(mtx_debugger_signal);

			run_mode = RunMode::FreeRunning;
			execution_state = ExecutionState::Running;
		}

		debugger_signal.notify_all();
	}

	// Jump to next instruction
	void stepOver() {
		if (run_mode != RunMode::Step)
			throw std::runtime_error("Cannot resume execution because it is already running");

		{
			std::unique_lock<std::mutex> source_lock(mtx_debugger_signal);

			execution_state = ExecutionState::Running;
		}

		debugger_signal.notify_all();

		// Wait for next instruction
		{
			std::unique_lock<std::mutex> debugger_lock(mtx_source_signal);

			source_signal.wait(debugger_lock, [this]() {return execution_state == ExecutionState::Stopped; });
		}
	}
};