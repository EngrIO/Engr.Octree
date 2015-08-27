#version 400
precision highp float; 

in FragmentData
{
    vec3 normal;
    vec4 color;
} frag;


void main()
{
    gl_FragColor = frag.color;
}