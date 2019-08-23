#include "context.h"
#include "render.h"
#include "loader.h"

#include <string>
#include <iostream>

int main() {

	context game;
	game.init(640, 480, "Source3");

#ifdef _DEBUG
	glEnable(GL_DEBUG_OUTPUT);
	glEnable(GL_DEBUG_OUTPUT_SYNCHRONOUS);

	glDebugMessageCallback(MessageCallback, 0);
	glDebugMessageControl(GL_DONT_CARE, GL_DONT_CARE, GL_DONT_CARE, 0, 0, true);
#endif

	// CCW Triangle
	vertex triVerts[] = 
	{
		{ {-0.5f, -0.5f, 0, 1}, {0, 1, 0, 1} },
		{ {0.5f,  -0.5f, 0, 1}, {1, 0, 0, 1} },
		{ {0,      0.5f, 0, 1}, {0, 0, 1, 1} }
	};

	unsigned int triIndices[] = { 0, 1, 2 };

	geometry triangle = makeGeometry(triVerts, 3, triIndices, 3);

	std::string basicVert = loadShader("basicVert.txt");

	std::string basicFrag = loadShader("basicFrag.txt");

	shader basicShad = makeShader(basicVert.c_str(), basicFrag.c_str());
	
	std::cout << basicVert << std::endl;

	while (!game.shouldClose())
	{
		game.tick();
		game.clear();

		draw(basicShad, triangle);
	}

	freeGeometry(triangle);
	freeShader(basicShad);
	game.term();

	return 0;
}