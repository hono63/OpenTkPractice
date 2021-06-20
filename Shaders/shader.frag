#version 330

out vec4 outputColor;
// declared in shader.vert   
in vec4 vertexColor;

void main()
{
    //outputColor = vec4(1.0, 1.0, 0.0, 1.0);
    outputColor = vertexColor;
}