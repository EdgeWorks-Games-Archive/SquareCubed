#version 330 core

// Interpolated values from the vertex shaders
in vec2 UV;

// Ouput data
out vec4 color;

// Constant values
uniform sampler2D textureSampler;

void main()
{
	// Sample the texture at the specified coordinates
	color = texture2D(textureSampler, UV);
}