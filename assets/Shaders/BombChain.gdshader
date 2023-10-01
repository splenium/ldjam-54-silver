shader_type spatial;
render_mode unshaded, blend_mix;

uniform sampler2D _texture : repeat_enable;
vec2 mirrorRepeat(vec2 texCoord) {
    // Calculate the fractional part of the texture coordinate
    vec2 fracCoord = fract(texCoord);

    // Determine if we need to mirror in the x and y directions
    vec2 mirrorFactor = step(0.5, fracCoord);

    // Mirror the fractional coordinate if necessary
    vec2 mirroredCoord = mix(fracCoord, 1.0 - fracCoord, mirrorFactor);

    // Scale the mirrored coordinate by the texture size to get the final coordinate
    return mirroredCoord;
}
varying vec3 world_position;

void vertex()
{
    world_position = (MODEL_MATRIX * vec4(VERTEX, 1.0)).xyz;
}

void fragment() 
{
	vec2 uv = UV;
	float cl = .05;
	uv.x += sin(uv.y*5.+TIME+world_position.x+world_position.y)*.01;
	uv.x = clamp(uv.x-.5, 0, cl);
	vec4 col = texture(_texture, mirrorRepeat(uv*20.)); // Don't know why repeat_enable is not working
	ALBEDO = col.xyz;
	ALPHA = col.w;
}

