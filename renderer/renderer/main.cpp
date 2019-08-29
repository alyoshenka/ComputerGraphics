#include "context.h"
#include "render.h"
#include "loader.h"
#include "geometry.h"
#include "time.h"

#include "glm/glm.hpp"
#include "glm/ext.hpp"

#include <string>
#include <iostream>
#include <math.h>
#include <cmath>

int main() 
{

	timeclock t;

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

	geometry triangle = makeGeometry(triVerts, 3, triIndices, 3);
	// geometry quad = makeGeometry(quadVerts, 4, quadIndices, 6);
	// geometry customQuad = makePlane(1.5f, 0.8f);
	// geometry obj = loadObj("Geometry/tri.obj");
	geometry cust = loadObj("Geometry/soulspear.obj");

	shader basicShad = makeShader(load("Shaders/basicVert.txt").c_str(), load("Shaders/basicFrag.txt").c_str());
	shader colorShad = makeShader(load("Shaders/colorVert.txt").c_str(), load("Shaders/colorFrag.txt").c_str());
	shader camShad = makeShader(load("Shaders/camVert.txt").c_str(), load("Shaders/camFrag.txt").c_str());
	shader uvShad = makeShader(load("Shaders/uvVert.txt").c_str(), load("Shaders/uvFrag.txt").c_str());
	shader lightShad = makeShader(load("Shaders/lightVert.txt").c_str(), load("Shaders/lightFrag.txt").c_str());

	glm::mat4 triModel = glm::identity<glm::mat4>();
	glm::mat4 camProj = glm::perspective(glm::radians(45.f), 640.f / 480.f, 0.1f, 100.f);
	glm::mat4 camView = glm::lookAt(glm::vec3(0, 0, -25), glm::vec3(0, 0, 0), glm::vec3(0, 1, 0));

	texture tex = loadTexture("Assets/soulspear_diffuse.tga");
	texture tex2 = loadTexture("Assets/x.png");
	texture tex3 = loadTexture("Assets/splat.png");

	light sun;
	sun.dir = glm::vec4{ 0, 0, 1, 1 };
	sun.col = glm::vec4{ 1, 1, 0, 1 };

	setUniform(camShad, 0, tex, 0);
	setUniform(camShad, 1, tex2, 1);
	setUniform(camShad, 2, tex3, 2);

	setUniform(lightShad, 0, camProj);
	setUniform(lightShad, 1, camView);
	setUniform(lightShad, 2, triModel);

	setUniform(lightShad, 3, tex, 0);
	setUniform(lightShad, 4, sun.dir);
	setUniform(lightShad, 5, sun.col);

	float angle = 0;
	float scaleNumber = 200;
	float scaleCount = 0;
	float scaleValue = 0.01;
	float scaleCurrent = 0.995;

	float lerp = 0;
	float dir = 0.01f;	

	while (!game.shouldClose())
	{
		game.tick();
		game.clear();

		/*t.tick();
		std::cout << t.deltaTime() << std::endl;
		*/

		// triModel = glm::translate(triModel, glm::vec3(cos(angle) / 20, sin(angle) / 20, 0));
		// angle += 0.1f;

		triModel = glm::rotate(triModel, glm::radians(1.f), glm::vec3(0, 1, 0));

		/*triModel = glm::scale(triModel, glm::vec3(scaleCurrent, 1, 1));
		scaleCount++;
		if (scaleCount > scaleNumber)
		{
			scaleCount = 0;
			if (scaleCurrent > 1)
			{
				scaleCurrent = 1 - scaleValue;
			}
			else
			{
				scaleCurrent = 1 + scaleValue;
			}
		}*/
		
		/*
		setUniform(camShad, 0, camProj);
		setUniform(camShad, 1, camView);
		*/
		// setUniform(camShad, 2, triModel);

		/*
		setUniform(lightShad, 0, camProj);
		setUniform(lightShad, 1, camView);
		*/
		setUniform(lightShad, 2, triModel);

		 // draw(camShad, quad);
		 // draw(colorShad, triangle);
		 // draw(lightShad, triangle);
		 // draw(basicShad, customQuad);

		// draw(lightShad, triangle);
		draw(lightShad, cust);

		// assert(glGetError() == GL_NO_ERROR);

		/*GLint loc = glGetUniformLocation(uvShad.program, "val");
		if (loc != -1)
		{
			lerp += 0.001f;
			glUniform1f(loc, (float)sin(lerp));
			std::cout << loc << std::endl;
		}
		else
		{
			std::cout << loc << std::endl;
		}

		draw(uvShad, triangle);*/
	}

	freeGeometry(triangle);
	// freeGeometry(quad);
	// freeGeometry(customQuad);
	freeShader(basicShad);
	freeShader(colorShad);
	freeShader(camShad);
	freeTexture(tex);
	freeTexture(tex2);
	freeTexture(tex3);
	game.term();

	return 0;
}