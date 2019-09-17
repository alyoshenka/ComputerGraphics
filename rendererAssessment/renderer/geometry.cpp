#include "geometry.h"

#define TINYOBJLOADER_IMPLEMENTATION // define this in only *one* .cc
#include "tinyobjectloader/tiny_obj_loader.h"

#include <iostream>
#include <cstdlib>
#include <ctime>

geometry loadObj(const char * fileName) // NOT DONE
{
	std::vector<vertex> verts;
	std::vector<unsigned> indices;
	size_t vertCount;
	size_t indexCount;

	tinyobj::attrib_t attrib;
	std::vector<tinyobj::shape_t> shapes;
	std::vector<tinyobj::material_t> materials;

	std::string warn;
	std::string err;

	tinyobj::LoadObj(&attrib, &shapes, &materials, &warn, &err, fileName);

	bool ret = tinyobj::LoadObj(&attrib, &shapes, &materials, &warn, &err, fileName);

	if (!warn.empty())
	{
		std::cout << warn << std::endl;
	}

	if (!err.empty())
	{
		std::cerr << err << std::endl;
	}

	if (!ret)
	{
		exit(1);
	}

	// Loop over shapes
	for (size_t s = 0; s < shapes.size(); s++)
	{
		// Loop over faces(polygon)
		size_t index_offset = 0;
		for (size_t f = 0; f < shapes[s].mesh.num_face_vertices.size(); f++)
		{
			int fv = shapes[s].mesh.num_face_vertices[f];

			// Loop over vertices in the face.
			for (size_t v = 0; v < fv; v++)
			{
				// access to vertex
				tinyobj::index_t idx = shapes[s].mesh.indices[index_offset + v];
				tinyobj::real_t vx = attrib.vertices[3 * idx.vertex_index + 0];
				tinyobj::real_t vy = attrib.vertices[3 * idx.vertex_index + 1];
				tinyobj::real_t vz = attrib.vertices[3 * idx.vertex_index + 2];
				tinyobj::real_t nx = attrib.normals[3 * idx.normal_index + 0];
				tinyobj::real_t ny = attrib.normals[3 * idx.normal_index + 1];
				tinyobj::real_t nz = attrib.normals[3 * idx.normal_index + 2];
				tinyobj::real_t tx = attrib.texcoords[2 * idx.texcoord_index + 0];
				tinyobj::real_t ty = attrib.texcoords[2 * idx.texcoord_index + 1];
				// Optional: vertex colors
				tinyobj::real_t red = attrib.colors[3*idx.vertex_index+0];
				tinyobj::real_t green = attrib.colors[3*idx.vertex_index+1];
				tinyobj::real_t blue = attrib.colors[3*idx.vertex_index+2];

				vertex vert;
				vert.pos = { vx, vy, vz, 1 };
				vert.norm = { nx, ny, nz, 0 };
				vert.col = { red, green, blue, 1 };
				vert.uv = { tx, ty };
				verts.push_back(vert);
				indices.push_back(3 * f + v); // why this
			}
			index_offset += fv;

			// per-face material
			shapes[s].mesh.material_ids[f];
		}
	}

	vertCount = verts.size();
	indexCount = indices.size();

	return makeGeometry(&verts[0], vertCount, &indices[0], indexCount);
}

// origin in upper left
// ERROR CHECKING
// challenge: normal support
geometry makePlane(float width, float height, int rows, int cols, bool randomizeColors)
{
	// requirements:
	// w, h >= 1
	// r, c > 0

	int vertCount = (rows + 1) * (cols + 1);
	int indcCount = rows * cols * 6;
	float rowIncr = width / rows;
	float colIncr = height / cols;

	vertex * verts = new vertex[vertCount];
	int idx = 0;
	for (int y = 0; y <= cols; y++) 
	{
		for (int x = 0; x <= rows; x++) 
		{
			verts[idx++] = 
			{ 
				{ rowIncr * x, -colIncr * y, 0, 1 }, // pos
				{ 0, 0, 1, 0 }, // norm
				{ 1, 0, 0, 1}, // col red
				{ (1.0f * x) / rows, (1.0f * y) / cols } // uv
			};
		}
	}
	if (randomizeColors)
	{
		srand(time(NULL));
		for (int i = 0; i < vertCount; i++)
		{
			verts[i].col = 
			{
				(rand() % 11) / 10.0f,
				(rand() % 11) / 10.0f,
				(rand() % 11) / 10.0f,
				1
			};
		}
	}

	for (int i = 0; i < vertCount; i++)
	{
		std::cout << verts[i].pos.x << ", " << verts[i].pos.y << std::endl;
	}

	unsigned int * indcs = new unsigned int[indcCount];
	idx = 0;
	int blockNum = 1;

	for (int y = 0; y < cols; y++)
	{
		for (int x = 0; x < rows; x++)
		{
			int startNum = (y + 1) * (rows + 1) + x;

			// top left triangle
			indcs[idx++] = startNum; 			std::cout << indcs[idx - 1] << " ";
			indcs[idx++] = blockNum;  			std::cout << indcs[idx - 1] << " ";
			indcs[idx++] = blockNum - 1; 		std::cout << indcs[idx - 1];

			std::cout << std::endl;

			// bottom right triangle
			indcs[idx++] = startNum;			std::cout << indcs[idx - 1] << " ";
			indcs[idx++] = startNum + 1;		std::cout << indcs[idx - 1] << " ";
			indcs[idx++] = blockNum;			std::cout << indcs[idx - 1];

			blockNum++;

			std::cout << std::endl;
			std::cout << std::endl;
		}
		blockNum++;
	}

	geometry quad = makeGeometry(verts, vertCount, indcs, indcCount);

	return quad;
}
