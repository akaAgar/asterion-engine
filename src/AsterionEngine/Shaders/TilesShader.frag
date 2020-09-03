/*
==========================================================================
This file is part of Asterion Engine, an OpenGL/OpenTK 1-bit graphic
engine by @akaAgar (https://github.com/akaAgar/one-bit-of-engine)
Asterion Engine is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.
Asterion Engine is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.
You should have received a copy of the GNU General Public License
along with Asterion Engine. If not, see https://www.gnu.org/licenses/
==========================================================================
*/

#version 330 core

uniform int animationFrame;
uniform float time;

uniform sampler2D texture0;
uniform sampler2D texture1;
uniform sampler2D texture2;
uniform sampler2D texture3;

in vec3 fragColor;
in vec2 fragUV;
in float fragTileMap;

out vec4 color;

void main()
{
  if (fragTileMap > 2.9)
    color = texture(texture3, fragUV);
  else if (fragTileMap > 1.9)
    color = texture(texture2, fragUV);
  else if (fragTileMap > 0.9)
    color = texture(texture1, fragUV);
  else
    color = texture(texture0, fragUV);
 
  float brightness = 0;

  switch (animationFrame)
  {
    default: brightness = color.r; break;
    case 1: brightness = color.g; break;
    case 2: brightness = color.b; break;
  }

  color = vec4(brightness, brightness, brightness, 1) * vec4(fragColor, 1);
  color.a = color.r;
}
