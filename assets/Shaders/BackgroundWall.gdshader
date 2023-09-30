shader_type spatial;
render_mode unshaded;

#define sat(a) clamp(a, 0., 1.)

varying vec3 world_position;

void vertex()
{
    world_position = (MODEL_MATRIX * vec4(VERTEX, 1.0)).xyz;
}

void fragment() {
	vec2 uv = world_position.xy;
	vec3 col = vec3(0., 0., 0.);
	
	float rep = 1.;
	uv.x = mod(uv.x+rep*.5,rep)-rep*.5;
	uv.y -= .2;
	
	float circle = length(uv)-0.2;
	float an = atan(uv.y, uv.x)+pow(length(uv)*4.,5.);
	float finf = sin(an*10.-TIME*2.);
	float border = abs(circle)-.002;
	vec3 fins = mix(vec3(0.1), vec3(.5,.6,.7)*2.,sat(.5*(1.-sat(border*100.))+sat((finf-.5)*2.)));
	
	float innerfin = length(uv)-.05;
	fins = mix(fins, vec3(0.), 1.-sat(innerfin*100.));
	col = mix(col, fins, 1.-sat(circle*500.));
	
	ALBEDO = col;
}
