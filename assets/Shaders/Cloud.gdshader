shader_type spatial;
render_mode unshaded, blend_mix;

uniform sampler2D _voronoiseTex;
uniform vec4 _color : source_color = vec4(1.);

#define sat(a) clamp(a, 0., 1.)

varying vec3 world_position;

void vertex()
{
    world_position = (MODEL_MATRIX * vec4(VERTEX, 1.0)).xyz;
}

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

#define textureMirrorRepeat(sampler, uv) texture(sampler, mirrorRepeat(uv))
#define rot(a) mat2(vec2(cos(a), -sin(a)), vec2(sin(a), cos(a)))

vec4 cloud(vec2 uv, vec3 lightcol)
{
	vec3 col = lightcol;//vec3(.7,.8,.9);
	col *= mix(vec3(.3), vec3(1.), 
	textureMirrorRepeat(_voronoiseTex, uv+vec2(0., world_position.z)).x);
	
	uv *= 3.;
	float t = TIME*.05+world_position.z;

	float alpha = textureMirrorRepeat(_voronoiseTex, uv*.75+vec2(sin(-.3*t), sin(t*.289))*.2).x;
	alpha *= .5+textureMirrorRepeat(_voronoiseTex, uv*rot(.5)*.5+vec2(sin(t), sin(t*.89))).x;
	alpha *= .75+textureMirrorRepeat(_voronoiseTex, uv*rot(2.)*.25+vec2(sin(t), sin(t*.89))).x;
	alpha = sat(alpha);
	return vec4(col, alpha);
}

void fragment() {
	vec2 uv = UV;//world_position.xy;
	vec4 cola = cloud(uv, vec3(.7,.8,.9));
	vec4 colb = sat(cloud(uv-vec2(.1), vec3(0.93f, 0.76f, 0.26f))-.5);
	cola.xyz += colb.xyz;
	vec3 outCol = cola.xyz*1.5;
	outCol = mix(outCol, vec3(1.)*(outCol.x+outCol.y+outCol.z)/3., .5);
	ALBEDO = outCol*_color.xyz;
	ALPHA = cola.a*_color.w;
}