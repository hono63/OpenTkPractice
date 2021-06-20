#version 330

out vec4 outputColor;
// declared in shader.vert   
//in vec4 vertexColor;
//in vec3 ourColor;
uniform vec4 uni4Color;

void main()
{
    //outputColor = vec4(1.0, 1.0, 0.0, 1.0);
    //outputColor = vertexColor;
    //outputColor = vec4(ourColor, 1.0);
    outputColor = uni4Color;
}