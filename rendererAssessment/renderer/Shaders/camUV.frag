#version 330

layout (location = 3) uniform sampler2D albedo;

in vec2 vUV;

out vec4 vertColor;

void main() 
{ 
	vertColor = texture(albedo, vUV); 
}