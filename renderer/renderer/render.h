#pragma once

#include "glew/GL/glew.h"
#include "glm/glm.hpp" // not ideal

struct vertex
{
	glm::vec4 pos;
	glm::vec4 color;
};

struct geometry
{
	GLuint vao, vbo, ibo; // buffers
	GLuint size; // index count
};

struct shader
{
	GLuint program;
};

geometry makeGeometry(vertex* verts, size_t vertCount, unsigned* indices, size_t indexCount);

void freeGeometry(geometry& geo);

shader makeShader(const char* vertSource, const char* fragSource);

void freeShader(shader& shad);

void draw(const shader& shad, const geometry& geo);

GLDEBUGPROC errorDisplay();

void GLAPIENTRY MessageCallback(GLenum source,
	GLenum type,
	GLuint id,
	GLenum severity,
	GLsizei length,
	const GLchar* message,
	const void* userParam);

