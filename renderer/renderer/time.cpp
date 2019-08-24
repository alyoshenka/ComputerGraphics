#include "time.h"

#include <chrono>
#include <ctime>

#include <iostream>


timeclock::timeclock()
{
	totalTime = 0;
	lastDeltaTime = 0;

	startTime = std::chrono::system_clock::now().time_since_epoch() /
		std::chrono::milliseconds(1);
}

timeclock::~timeclock()
{
}

float timeclock::systemTime() const
{
	std::chrono::system_clock::time_point now = std::chrono::system_clock::now();
	std::chrono::duration<float, std::milli> ms = now.time_since_epoch();
	return (std::chrono::duration_cast<std::chrono::seconds>
		(std::chrono::system_clock::now().time_since_epoch())).count();
}

float timeclock::deltaTime() const
{
	return lastDeltaTime;
}

void timeclock::resetTime()
{
	totalTime = lastDeltaTime = 0;
}

void timeclock::tick()
{
	float currentTime = std::chrono::system_clock::now().time_since_epoch() /
		std::chrono::milliseconds(1);
	lastDeltaTime = currentTime - lastDeltaTime;
	totalTime += lastDeltaTime;
}

