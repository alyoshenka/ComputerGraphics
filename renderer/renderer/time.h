#pragma once

class timeclock
{
private:
	float totalTime; // time since start
	float lastDeltaTime; // time since last frame
	float startTime; // time at start of program
public:
	timeclock();
	~timeclock();

	float systemTime() const;
	float deltaTime() const;

	void resetTime();
	void tick(); // update frame time
};
