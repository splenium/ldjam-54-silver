shader_type spatial;
render_mode unshaded, blend_mix;

uniform sampler2D _mainTex;
#define sat(a) clamp(a, 0., 1.)

void vertex()
{

}

void fragment() {
	vec2 uv = UV;
	vec4 col = texture(_mainTex, uv).xyzw;
	col.xyz *= mix(.2,1.,sat((-uv.y+.7)*2.));

	ALBEDO = col.xyz;
	ALPHA = col.w;
}
