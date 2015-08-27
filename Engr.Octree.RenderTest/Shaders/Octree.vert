#version 400
precision highp float;

layout (location = 0) in vec3 position; 
layout (location = 1) in vec4 color; 
layout (location = 2) in float size; 

uniform mat4 mvp;

out Data
{
  vec4 color;
  float size;
} v;

void main(void) 
{ 
    gl_Position = mvp * vec4(position, 1.0);
    v.size = size;
    v.color = color;
}