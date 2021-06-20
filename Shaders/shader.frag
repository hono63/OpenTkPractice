#version 330

out vec4 outputColor;
// declared in shader.vert   
//in vec4 vertexColor;
//in vec3 ourColor;
in vec2 texCoord;

//uniform vec4 uni4Color;
uniform sampler2D texture0;
uniform sampler2D texture1;

void main()
{
    //outputColor = vec4(1.0, 1.0, 0.0, 1.0);
    //outputColor = vertexColor;
    //outputColor = vec4(ourColor, 1.0);
    //outputColor = uni4Color;
    //outputColor = texture(texture0, texCoord);
    outputColor = mix(texture(texture0, texCoord), texture(texture1, texCoord), 0.2);
}