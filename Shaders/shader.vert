#version 330 core

// attributes from Window.cs 
layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec3 aColor;

out vec4 vertexColor;
out vec3 ourColor;

void main(void)
{
	gl_Position = vec4(aPosition, 1.0);
	// dark red color
	vertexColor = vec4(0.5, 0.0, 0.0, 1.0);
	ourColor = aColor;
}