#include "context.h"
#include "render.h"
#include "loader.h"
#include "geometry.h"
#include "time.h"
#include "math.h"

#include "glm/glm.hpp"
#include "glm/ext.hpp"
#include "glfw/glfw3.h" 

#include <string>
#include <iostream>
#include <vector>

int main() 
{
	int w = 640;
	int h = 480;
	context game;
	game.init(w, h, "Source3");

#ifdef _DEBUG
	glEnable(GL_DEBUG_OUTPUT);
	glEnable(GL_DEBUG_OUTPUT_SYNCHRONOUS);

	glDebugMessageCallback(MessageCallback, 0);
	glDebugMessageControl(GL_DONT_CARE, GL_DONT_CARE, GL_DONT_CARE, 0, 0, true);
#endif

	// custom geometry
	// CCW Triangle
	vertex triVerts[] =
	{
		{ {-0.5f, -0.5f, 0, 1}, { 0, 0, 1, 0 }, { 0, 1, 0, 1 }, {0.f, 0.f} },
		{ {0.5f,  -0.5f, 0, 1}, { 0, 0, 1, 0 }, { 1, 0, 0, 1 }, {1.f, 0.f} },
		{ {0,      0.5f, 0, 1}, { 0, 0, 1, 0 }, { 0, 0, 1, 1 }, {.5f, 1.f} }
	};

	vertex quadVerts[] = 
	{
		{ {-0.5f, -0.5f, 0, 1}, {0, 1, 0, 1} },
		{ {0.5f,  -0.5f, 0, 1}, {1, 0, 0, 1} },
		{ {0,      0.5f, 0, 1}, {0, 0, 1, 1} },
		{ {0,     -0.8f, 0, 1}, {0, 0, 1, 1} }
	};

	unsigned int triIndices[] = { 0, 1, 2};
	unsigned int quadIndices[] = { 3, 2, 0, 3, 1, 2 };

	// create geometry
	geometry triangle = makeGeometry(triVerts, 3, triIndices, 3);
	geometry quad = makeGeometry(quadVerts, 4, quadIndices, 6);
	geometry customQuad = makePlane(1.5f, 0.8f);
	geometry cust = loadObj("Geometry/teapot.obj");

	// load shaders
	shader basicShad = makeShader(load("Shaders/basicVert.txt").c_str(), load("Shaders/basicFrag.txt").c_str());
	shader colorShad = makeShader(load("Shaders/colorVert.txt").c_str(), load("Shaders/colorFrag.txt").c_str());
	shader camShad = makeShader(load("Shaders/camVert.txt").c_str(), load("Shaders/camFrag.txt").c_str());
	shader uvShad = makeShader(load("Shaders/uvVert.txt").c_str(), load("Shaders/uvFrag.txt").c_str());
	shader lightShad = makeShader(load("Shaders/lightVert.txt").c_str(), load("Shaders/lightFrag.txt").c_str());

	// set up camera
	glm::mat4 triModel = glm::identity<glm::mat4>();
	glm::mat4 camProj = glm::perspective(glm::radians(45.f), 640.f / 480.f, 0.1f, 100.f);
	glm::mat4 camView = glm::lookAt(glm::vec3(0, 0, -40), glm::vec3(0, 0, 0), glm::vec3(0, 1, 0));

	// lad textures
	texture tex = loadTexture("Assets/soulspear_diffuse.tga");
	texture tex2 = loadTexture("Assets/x.png");
	texture tex3 = loadTexture("Assets/splat.png");
	texture marble = loadTexture("Assets/paint.jfif");

	light sun;
	sun.dir = glm::vec4{ 0, 0, 1, 1 };
	sun.col = glm::vec4{ 1, 0, 1, 1 };

	// set shader uniforms
	setUniform(camShad, 0, tex, 0);
	setUniform(camShad, 1, tex2, 1);
	setUniform(camShad, 2, tex3, 2);

	setUniform(camShad, 0, camProj);
	setUniform(camShad, 1, camView);
	setUniform(camShad, 2, triModel);

	setUniform(lightShad, 0, camProj);
	setUniform(lightShad, 1, camView);
	setUniform(lightShad, 2, triModel);

	setUniform(lightShad, 3, marble, 0);
	setUniform(lightShad, 4, sun.dir);
	setUniform(lightShad, 5, sun.col);

	// glfw
	GLdouble x = 0;
	GLdouble y = 0;
	int shadowMax = 4;
	float xDif;
	float yDif;
	float xVal;
	float yVal;

	// enable cycling
	std::vector<geometry> geos;
	std::vector<shader> shads;

	geos.push_back(triangle);
	geos.push_back(quad);
	geos.push_back(customQuad);
	geos.push_back(cust);

	shads.push_back(basicShad);
	shads.push_back(colorShad);
	// shads.push_back(camShad);
	shads.push_back(uvShad);
	shads.push_back(lightShad);

	int geoIdx = 0;
	int shadIdx = 0;
	int frameWait = 50;
	int frameElapsed = 50;

	while (!game.shouldClose())
	{
		game.tick();
		game.clear();

		// move camera
		triModel = glm::rotate(triModel, glm::radians(1.f), glm::vec3(0, 1, 0));
		setUniform(lightShad, 2, triModel);

		// move directional light
		glfwGetCursorPos(game.window, &x, &y);
		xDif = invlerp(0, w, x);
		yDif = invlerp(0, h, y);
		xVal = mylerp(-shadowMax, shadowMax, xDif);
		yVal = mylerp(-shadowMax, shadowMax, yDif);
		sun.dir.x = xVal;
		sun.dir.y = yVal;
		setUniform(lightShad, 4, sun.dir);

		// check for changes
		int state = glfwGetMouseButton(game.window, GLFW_MOUSE_BUTTON_LEFT);
		if (frameElapsed > frameWait && state == GLFW_PRESS)
		{
			geoIdx++;
			frameElapsed = 0;
			if (geoIdx >= geos.size())
			{
				geoIdx = 0;
			}
		}
		state = glfwGetMouseButton(game.window, GLFW_MOUSE_BUTTON_RIGHT);
		if (frameElapsed > frameWait && state == GLFW_PRESS)
		{
			shadIdx++;
			frameElapsed = 0;
			if (shadIdx >= shads.size())
			{
				shadIdx = 0;
			}
		}
		frameElapsed++;

		// draw(shads[shadIdx], geos[geoIdx]);

		// draw(camShad, triangle);
		draw(colorShad, triangle);
		// draw(lightShad, triangle);
		// draw(basicShad, customQuad);
	    // draw(lightShad, triangle);
	}

	freeGeometry(triangle);
	freeGeometry(quad);
	freeGeometry(customQuad);
	freeShader(basicShad);
	freeShader(colorShad);
	freeShader(camShad);
	freeTexture(tex);
	freeTexture(tex2);
	freeTexture(tex3);
	freeTexture(marble);
	game.term();

	return 0;
}

