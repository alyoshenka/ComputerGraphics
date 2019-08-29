#include "geometry.h"

#define TINYOBJLOADER_IMPLEMENTATION // define this in only *one* .cc
#include "tinyobjectloader/tiny_obj_loader.h"

#include <iostream>

geometry loadObj(const char * fileName) // NOT DONE
{
	tinyobj::attrib_t attrib;
	std::vector<tinyobj::shape_t> shapes;
	std::vector<tinyobj::material_t> materials;

	std::string warn;
	std::string err;

	tinyobj::LoadObj(&attrib, &shapes, &materials, &warn, &err, fileName);

	vertex* verts = nullptr;
	size_t vertCount = 0;
	unsigned* indices = nullptr;
	size_t indexCount = 0;

	// get vert count
	vertCount = attrib.vertices.size() / 3; // triangles

	// pass in verts
	verts = new vertex[vertCount];
	vertex cur;
	int posIdx = 0;
	int normIdx = 0;
	int uvIdx = 0;
	int colIdx = 0;

	for (int i = 0; i < vertCount; i++)
	{
		cur = verts[i];

		cur.pos.x = attrib.vertices[posIdx++];
		cur.norm.x = attrib.normals[normIdx++];
		cur.uv.x = attrib.texcoords[uvIdx++];
		cur.col.r = attrib.colors[colIdx++];

		cur.pos.y = attrib.vertices[posIdx++];
		cur.norm.y = attrib.normals[normIdx++];
		cur.uv.y = attrib.texcoords[uvIdx++];
		cur.col.g = attrib.colors[colIdx++];

		cur.pos.z = attrib.vertices[posIdx++];
		cur.norm.z = attrib.normals[normIdx++];
		cur.col.b = attrib.colors[colIdx++];

		verts[i] = cur;
	}

	// get index count
	indexCount = shapes.size();

	// pass in indices
	indices = new unsigned[indexCount];
	for (int i = 0; i < indexCount; i++)
	{
		indices[i] = shapes[0].mesh.indices[i].vertex_index;
	}

	return makeGeometry(verts, vertCount, indices, indexCount);
}

geometry makePlane(float width, float height) // finish
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
