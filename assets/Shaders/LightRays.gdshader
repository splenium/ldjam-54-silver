shader_type spatial;
render_mode unshaded, blend_add;

uniform sampler2D _voronoiseTex;
uniform sampler2D _noiseTex;

#define sat(a) clamp(a, 0., 1.)

varying vec3 world_position;

void vertex()
{
    world_position = (MODEL_MATRIX * vec4(VERTEX, 1.0)).xyz;
}
vec3 post(vec2 uv,vec3 rd)
{
	vec2 ouv = uv;
  vec3 col = rd;
  vec3 blue =vec3(89.,151.,255.)/255.;
uv *= 2.1+world_position.z;

  col += blue*(sin(uv.x+uv.y)*.5+.5);
  float an = uv.x+uv.y*.5;
  col += sat(uv.y)*blue*.5*sat((sin(an*25.+TIME)*.5+.5)+(sin(an*5.-TIME*.5)*.5+.5));


  return col*(1.-sat(ouv.y-.1));
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
	vec3 rays = vec3(0.);

	rays = post(uv, rays);
  vec3 blue =vec3(89.,151.,255.)/255.;
vec3 green = vec3(.2,.45,.3);
vec3 res = mix(green, blue, textureMirrorRepeat(_voronoiseTex, uv+vec2(0., world_position.z)).x);
  float n = textureMirrorRepeat(_noiseTex,vec2(sin(uv.y*5.+TIME+sin(uv.y*15.))*.05+uv.x,uv.y-TIME*.01)).x;
 res += textureMirrorRepeat(_voronoiseTex, uv).x*.5*blue*float(n>.85)*(sin(uv.x+uv.y)*.5+.5);
	ALBEDO = res;
	
	ALPHA = rays.x*.75;
}