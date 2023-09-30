shader_type spatial;
render_mode unshaded, blend_mix;

uniform sampler2D _headBGTex;
uniform sampler2D _headOutlineTex;
uniform sampler2D _headHighLightTex;
uniform vec4 _lightColor : source_color = vec4(1.);

void fragment() {
	vec4 col = texture(_headBGTex, UV);
	float highlight = texture(_headHighLightTex, UV).a;
	float outline = texture(_headOutlineTex, UV).a;

	vec3 baseCol = vec3(0.22f, 0.2f, 0.2f)*1.8*_lightColor.xyz;	
	col.xyz *= mix(baseCol, vec3(1.)*(baseCol.x+baseCol.y+baseCol.z)/3., .5);
	col.xyz = mix(col.xyz, baseCol, highlight);
	col.xyz = mix(col.xyz, vec3(0.), outline);
	ALBEDO = col.xyz;
	ALPHA = max(col.w, outline);
}
