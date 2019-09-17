#pragma once

class context 
{
public:
	struct GLFWwindow * window; // should be private

	bool init(int width, int height, const char * title);
	void tick(); // update
	void term(); // terminate
	void clear();

	bool shouldClose() const;
	void getWindow(GLFWwindow* newWin) { newWin = window; };
};
