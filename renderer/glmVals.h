#pragma once

#include "glm/glm.hpp"

struct glmVals
{
public:
	union
	{
		struct
		{
			float r, g, b, a;
		};
		struct
		{
			float x, y, z, w;
		};
	};

	static glm::vec3 forward;
	static glm::vec3 backward;
	static glm::vec3 left;
	static glm::vec3 right;
	static glm::vec3 up;
	static glm::vec3 down;

	static glm::vec4 red;
	static glm::vec4 green;
	static glm::vec4 blue;
	static glm::vec4 white;
	static glm::vec4 black;
};
