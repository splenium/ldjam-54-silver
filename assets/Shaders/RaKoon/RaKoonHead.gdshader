shader_type spatial;
render_mode unshaded, blend_mix;

uniform sampler2D _headBGTex;
uniform sampler2D _headOutlineTex;
uniform sampler2D _headHighLightTex;

void fragment() {
	vec4 col = texture(_headBGTex, UV);
	float highlight = texture(_headHighLightTex, UV).a;
	float outline = texture(_headOutlineTex, UV).a;

	vec3 baseCol = vec3(.5,.4,.2);	
	col.xyz *= baseCol*.75;
	col.xyz = mix(col.xyz, baseCol, highlight);
	col.xyz = mix(col.xyz, vec3(0.), outline);
	ALBEDO = col.xyz;
	ALPHA = max(col.w, outline);
}
