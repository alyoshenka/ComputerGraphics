#pragma once

class context 
{
private:
	struct GLFWwindow * window;

public:
	bool init(int width, int height, const char * title);
	void tick(); // update
	void term(); // terminate
	void clear();

	bool shouldClose() const;
};
