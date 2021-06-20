#version 330 core

// attributes from Window.cs 
layout(location = 0) in vec3 aPosition;
//layout(location = 1) in vec3 aColor;
layout(location = 1) in vec2 aTexCoord;

out vec4 vertexColor;
//out vec3 ourColor;
out vec2 texCoord;

void main(void)
{
	texCoord = aTexCoord;
	gl_Position = vec4(aPosition, 1.0);
	//vertexColor = vec4(0.5, 0.0, 0.0, 1.0); // dark red color
	//ourColor = aColor;
}