#version 430

layout (location = 3) uniform sampler2D albedo;

layout (location = 4) uniform vec3 lightDir;
layout (location = 5) uniform vec4 lightColor;

layout (location = 6) uniform vec3 lightDir2;
layout (location = 7) uniform vec4 lightColor2;

layout (location = 8) uniform vec2 lightBlend;

in vec2 vUV;
in vec3 vNormal;

out vec4 vertColor;

void main() 
{ 
	vec4 mixedLightColor = mix(lightColor, lightColor2, lightBlend.y);

	float d = max(0, dot(vNormal, -lightDir));
	vec4 diffuseA = d * mixedLightColor;

	d = max(0, dot(vNormal, -lightDir2));
	vec4 diffuseB = d * mixedLightColor;

	vec4 diffuse = mix(diffuseA, diffuseB, lightBlend.x);

	vec4 base = texture(albedo, vUV);

	vertColor = vec4((diffuse * base).xyz, 1.0f); 
}