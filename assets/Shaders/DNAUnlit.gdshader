shader_type spatial;
render_mode unshaded, blend_mix;
uniform sampler2D albedo_texture;
#define sat(a) clamp(a, 0., 1.)

void fragment() {
	vec4 col = texture(albedo_texture, UV);
	ALBEDO = col.xyz;
	float shape = length(UV-.5)-.2;
	float alphamod = 1.-sat(shape/.2); // To fix semitransparency in texture
	ALPHA = col.w*alphamod;
}
