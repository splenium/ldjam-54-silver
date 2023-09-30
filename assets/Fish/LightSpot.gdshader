shader_type spatial;
render_mode unshaded, blend_mix;

uniform vec4 _lightColor : source_color = vec4(1.);
#define sat(a) clamp(a, 0., 1.)

void fragment() {
	ALBEDO = _lightColor.xyz;
	float shape = length(UV-.5)-.2;
	ALPHA = 1.-sat( length(UV-.5)/.5);
}
