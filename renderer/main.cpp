#include "context.h"
#include "render.h"
#include "loader.h"
#include "geometry.h"
#include "time.h"
#include "math.h"
#include "glmVals.h"

#include "glm/glm.hpp"
#include "glm/ext.hpp"
#include "glfw/glfw3.h" 

#include <string>
#include <iostream>
#include <vector>

// todo fix gitignore: debug etc
// assertions

int main() 
{
	int w = 800;
	int h = 400;
	context game;
	game.init(w, h, "Renderer Assessment");

#ifdef _DEBUG
	glEnable(GL_DEBUG_OUTPUT);
	glEnable(GL_DEBUG_OUTPUT_SYNCHRONOUS);

	glDebugMessageCallback(MessageCallback, 0);
	glDebugMessageControl(GL_DONT_CARE, GL_DONT_CARE, GL_DONT_CARE, 0, 0, true);
#endif

	geometry teapot = loadObj("Geometry/teapot.obj");
	geometry spear = loadObj("Geometry/soulspear.obj");

	// load shader
	shader lightShad = makeShader(load("Shaders/light.vert").c_str(), load("Shaders/lightBlend.frag").c_str());
	shader basicUV = makeShader(load("Shaders/camUV.vert").c_str(), load("Shaders/camUV.frag").c_str());

	// set up camera
	glm::mat4 triModel = glm::identity<glm::mat4>();
	glm::mat4 camProj = glm::perspective(glm::radians(45.f), 800.f / 400.f, 0.1f, 100.f);
	glm::mat4 camView = glm::lookAt(glm::vec3(0, 0, -40), glm::vec3(0, 0, 0), glm::vec3(0, 1, 0));

	// load textures
	texture spearTex = loadTexture("Assets/soulspear_diffuse.tga");
	texture splat = loadTexture("Assets/splat.png");
	texture marble = loadTexture("Assets/paint.jfif");

	// create sun
	light sun; 
	sun.dir = glmVals::forward;
	sun.col = glmVals::red;

	// set camera uniform
	setUniform(lightShad, 0, camProj);
	setUniform(lightShad, 1, camView);
	setUniform(lightShad, 2, glm::translate(triModel, { 10, 0, 0 }));

	// set light uniform
	setUniform(lightShad, 3, marble, 0);
	setUniform(lightShad, 4, sun.dir);
	setUniform(lightShad, 5, sun.col);
	setUniform(lightShad, 6, glmVals::up);
	setUniform(lightShad, 7, glmVals::blue);
	setUniform(lightShad, 8, { 0, 0.7 });

	// spear
	setUniform(basicUV, 0, camProj);
	setUniform(basicUV, 1, camView);
	setUniform(basicUV, 2, triModel);
	setUniform(basicUV, 3, spearTex, 1);

	// glfw
	GLdouble x = 0;
	GLdouble y = 0;
	int shadowMax = 4;
	float xDif;
	float yDif;
	float xVal;
	float yVal;

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

		// draw(lightShad, teapot);
		draw(lightShad, spear);
	}

	freeShader(basicUV);
	freeShader(lightShad);

	freeTexture(spearTex);
	freeTexture(splat);
	freeTexture(marble);

	freeGeometry(spear);
	freeGeometry(teapot);

	game.term();

	return 0;
}

