#include "render.h"

#include <iostream>

geometry makeGeometry(vertex* verts, size_t vertCount, unsigned* indices, size_t indexCount)
{
	// create instance of geometry
	geometry newGeo = {};
	newGeo.size = indexCount;

	// generate buffers
	glGenVertexArrays(1, & newGeo.vao); 	// vertex array object
	glGenBuffers(1, & newGeo.vbo); // vertex buffer object
	glGenBuffers(1, & newGeo.ibo); // index buffer object

	// bind buffers
	glBindVertexArray(newGeo.vao); // first
	glBindBuffer(GL_ARRAY_BUFFER, newGeo.vbo);
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, newGeo.ibo);

	// populate buffers
	glBufferData(GL_ARRAY_BUFFER, vertCount * sizeof(vertex), verts, GL_STATIC_DRAW);
	glBufferData(GL_ELEMENT_ARRAY_BUFFER, indexCount * sizeof(unsigned), indices, GL_STATIC_DRAW);

	// describe vertex data
	glEnableVertexAttribArray(0);
	glVertexAttribPointer(0, 4, GL_FLOAT, GL_FALSE, sizeof(vertex), (void*)0); // position[0] = [4] [floats] for [vertex] at [(void*)0] offset (needs to be normalized = [false])
	glEnableVertexAttribArray(1);
	glVertexAttribPointer(1, 4, GL_FLOAT, GL_FALSE, sizeof(vertex), (void*)16); 

	// unbind buffers (in a SPECIFIC order)
	glBindVertexArray(0); // first
	glBindBuffer(GL_ARRAY_BUFFER, 0);
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);

	// return the geometry
	return newGeo;
}

void freeGeometry(geometry& geo)
{
	glDeleteBuffers(1, & geo.vbo);
	glDeleteBuffers(1, & geo.ibo);
	glDeleteVertexArrays(1, & geo.vao);

	geo = {};
}

shader makeShader(const char* vertSource, const char* fragSource)
{
	// make the shader object
	shader newShad = {};
	newShad.program = glCreateProgram();

	// create the shaders
	GLuint vert = glCreateShader(GL_VERTEX_SHADER);
	GLuint frag = glCreateShader(GL_FRAGMENT_SHADER);

	// compile the shaders
	glShaderSource(vert, 1, &vertSource, 0);
	glShaderSource(frag, 1, &fragSource, 0);
	glCompileShader(vert);
	glDebugMessageCallback(errorDisplay(), (void*)0);
	glCompileShader(frag);
	glDebugMessageCallback(errorDisplay(), (void*)0);

	// attach the shaders
	glAttachShader(newShad.program, vert);
	glAttachShader(newShad.program, frag);

	// link the shaders
	glLinkProgram(newShad.program);
	glDebugMessageCallback(errorDisplay(), (void*)0);

	// delete the shaders
	glDeleteShader(vert);
	glDeleteShader(frag);

	// return the shader object
	return newShad;
}

void freeShader(shader& shad)
{
	glDeleteProgram(shad.program);
	shad = {};
}

void draw(const shader& shad, const geometry& geo)
{
	// bind the shader
	glUseProgram(shad.program);

	// bind the vao (geo and indices)
	glBindVertexArray(geo.vao);

	// draw
	glDrawElements(GL_TRIANGLES, geo.size, GL_UNSIGNED_INT, 0);
}

GLDEBUGPROC errorDisplay()
{
	GLenum err;
	while ((err = glGetError()) != GL_NO_ERROR)
	{
		std::cout << err << std::endl;
	}
	return GLDEBUGPROC();
}

void GLAPIENTRY MessageCallback(
	GLenum source,
	GLenum type,
	GLuint id,
	GLenum severity,
	GLsizei length,
	const GLchar* message,
	const void* userParam)
{
	std::cout << "\nSource: " << source << std::endl;
	std::cout << "Type: " << type << std::endl;
	std::cout << "ID: " << id << std::endl;
	std::cout << "Severity: " << severity << std::endl;
	std::cout << "Length: " << length << std::endl;
	std::cout << "Message: " << message << std::endl;
	std::cout << "userParam: " << userParam << std::endl;
}

