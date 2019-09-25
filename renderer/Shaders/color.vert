#version 430

layout (location = 0) in vec4 position;
layout (location = 2) in vec4 color;

out vec4 vColor;

void main() 
{ 
    gl_Position = position;
    vColor = color; 
}