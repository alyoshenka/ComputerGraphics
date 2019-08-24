#include "geometry.h"

#define TINYOBJLOADER_IMPLEMENTATION // define this in only *one* .cc
#include "tinyobjectloader/tiny_obj_loader.h"

#include <iostream>

geometry loadObj(const char * fileName)
{
	tinyobj::attrib_t attrib;
	std::vector<tinyobj::shape_t> shapes;
	std::vector<tinyobj::material_t> materials;

	std::string warn;
	std::string err;

	tinyobj::LoadObj(&attrib, &shapes, &materials, &warn, &err, fileName);

	// CCW Triangle
	vertex verts[] =
	{
		{ {-0.5f, -0.5f, 0, 1} },
		{ {0.5f,  -0.5f, 0, 1} },
		{ {0,      0.5f, 0, 1} }
	};
	unsigned int indices[] = { 0, 1, 2 };

	// return makeGeometry(verts, 3, indices, 3);
	return geometry();
}

geometry makePlane(float width, float height)
{
	width /= 2;
	height /= 2;

	// CW
	vertex quadVerts[] =
	{
		{ {-width, -height, 0, 1} },
		{ {-width,  height, 0, 1} },
		{ { width,  height, 0, 1} },
		{ { width, -height, 0, 1} }
	};

	unsigned int quadIndices[] = { 0, 2, 1, 0, 3, 2 };

	// geometry quad = makeGeometry(quadVerts, 4, quadIndices, 6);

	return geometry();
}
