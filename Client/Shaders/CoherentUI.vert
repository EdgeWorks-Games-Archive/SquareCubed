#version 330 core

// Input data
layout(location = 0) in vec3 vertexPosition; // Model-space position
layout(location = 1) in vec2 vertexUV;

// Output data
out vec2 UV;

void main()
{
	// Output position of the vertex in clip space
	gl_Position =  vec4(vertexPosition, 1);
	
	// Forward the uv coordinates to the fragment shader
	UV = vertexUV;
}