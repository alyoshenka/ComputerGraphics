#pragma once

#include "glew/GL/glew.h"
#include "glm/glm.hpp" // not ideal

struct vertex
{
	glm::vec4 pos;
	glm::vec4 norm;
	glm::vec4 col;
	glm::vec2 uv;
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

struct texture
{
	GLuint handle;
	unsigned width, height, channels;
};

struct light
{
	glm::vec3 dir;
	glm::vec4 col;
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

void setUniform(const shader &shad, GLuint location, const glm::mat4 &value);

void setUniform(const shader& shad, GLuint location, const texture &value, int textureSlot);

void setUniform(const shader& shad, GLuint location, const glm::vec3& value);

void setUniform(const shader & shad, GLuint location, const glm::vec4 & value);

texture makeTexture(unsigned width, unsigned height, unsigned channels, const unsigned char * pixels);

void freeTexture(texture &tex);

texture loadTexture(const char *imagePath);

