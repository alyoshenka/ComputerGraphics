#include "render.h"
#include "glm/gtc/type_ptr.hpp"
#define STB_IMAGE_IMPLEMENTATION
#include "stbimage/stb_image.h"

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
	// pos
	glEnableVertexAttribArray(0);
	glVertexAttribPointer(0, 4, GL_FLOAT, GL_FALSE, sizeof(vertex), (void*)0); // position[0] = [4] [floats] for [vertex] at [(void*)0] offset (needs to be normalized = [false])
	// norm
	glEnableVertexAttribArray(1);
	glVertexAttribPointer(1, 4, GL_FLOAT, GL_FALSE, sizeof(vertex), (void*)sizeof(vertex::pos));
	// col
	glEnableVertexAttribArray(2);
	glVertexAttribPointer(2, 4, GL_FLOAT, GL_FALSE, sizeof(vertex), (void*)(sizeof(vertex::pos) + sizeof(vertex::norm)));
	// uv
	glEnableVertexAttribArray(3);
	glVertexAttribPointer(3, 2, GL_FLOAT, GL_FALSE, sizeof(vertex), (void*)(sizeof(vertex::pos) + sizeof(vertex::norm) + sizeof(vertex::col)));

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

void setUniform(const shader &shad, GLuint location, const glm::mat4 &value)
{
	glProgramUniformMatrix4fv(shad.program, location, 1, GL_FALSE, glm::value_ptr(value));
}

void setUniform(const shader & shad, GLuint location, const texture & value, int textureSlot)
{
	// specify the texture slot we're working with
	glActiveTexture(GL_TEXTURE0 + textureSlot);

	// bind the texture to that slot
	glBindTexture(GL_TEXTURE_2D, value.handle);

	// assign the uniform to the shader
	glProgramUniform1i(shad.program, location, textureSlot);
}

void setUniform(const shader & shad, GLuint location, const glm::vec3 & value)
{
	glProgramUniform3fv(shad.program, location, 1, glm::value_ptr(value));
}

texture makeTexture(unsigned width, unsigned height, unsigned channels, const unsigned char *pixels)
{
	GLenum oglFormat = GL_RGBA; 
	switch (channels)
	{
	case 1: 
		oglFormat = GL_RED;
		break;
	case 2:
		oglFormat = GL_RG;
		break;
	case 3:
		oglFormat = GL_RGB;
		break;
	case 4:
		oglFormat = GL_RGBA;
		break;
	default:
		// ToDo: error handling
		break;
	}

	texture tex = { 0, width, height, channels }; // handle??

	// generate and bind texture
	// you have to bind the texture to perform operations on it
	glGenTextures(1, &tex.handle);
	glBindTexture(GL_TEXTURE_2D, tex.handle);

	// buffer/send actual data
	// texture target, lod level, internal pixel format, w, h, always 0, pixel format, pixel format pt 2, pixel array to load
	glTexImage2D(GL_TEXTURE_2D, 0, oglFormat, width, height, 0, oglFormat, GL_UNSIGNED_BYTE, pixels); 

	// describe how the texture will be used
	 // [linear] interpolation when going down [min]
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
	// [linear] interpolation when going up [mag]
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

	// set wrapping values
	// typical vec3(x, y, z) is vec3(s, t, r) in opengl
	// glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT); // repeat overlaps (1.5 -> 0.5)
	// glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_MIRRORED_REPEAT); // repeat but flip overlaps (1.5 -> -0.5 (backwards))
	// glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_R, GL_CLAMP_TO_BORDER); // anything outside 0-1 is a specified color

	// unbind
	glBindTexture(GL_TEXTURE_2D, 0);

	return tex;
}

void freeTexture(texture & tex)
{
	glDeleteTextures(1, &tex.handle);
	tex = {};
}

texture loadTexture(const char * imagePath)
{
	int imageWidth, imageHeight, imageFormat;
	unsigned char *rawPixelData = nullptr;

	// tell stb image to load the image
	stbi_set_flip_vertically_on_load(true);
	rawPixelData = stbi_load(imagePath, &imageWidth, &imageHeight, &imageFormat, STBI_default);

	// ToDO: ensure rawPixelData is NOT NULL, if null -> image failed to load
	if (nullptr == rawPixelData)
	{
		std::cout << "pixeldata null" << std::endl;
	}

	// pass the data to make the texture
	texture tex = makeTexture(imageWidth, imageHeight, imageFormat, rawPixelData);

	// free the image
	stbi_image_free(rawPixelData);

	return tex;
}

