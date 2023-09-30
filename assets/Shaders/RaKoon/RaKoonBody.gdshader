shader_type spatial;
render_mode unshaded, blend_mix;

uniform sampler2D _bodyBGTex;
uniform sampler2D _bodyHighlightTex1;
uniform sampler2D _bodyHighlightTex2;

void fragment() {
	vec4 col = texture(_bodyBGTex, UV);
	float highlight1 = texture(_bodyHighlightTex1, UV).a;
	float highlight2 = texture(_bodyHighlightTex2, UV).a;

	vec3 baseCol = vec3(.5,.4,.2);
	col.xyz *= baseCol*.75;
	col.xyz = mix(col.xyz, baseCol, highlight1); 
	col.xyz = mix(col.xyz, vec3(1.), highlight2);

	ALBEDO = col.xyz;
	ALPHA = col.w;
}
