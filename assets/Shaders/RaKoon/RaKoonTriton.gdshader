shader_type spatial;
render_mode unshaded, blend_mix;

uniform sampler2D _bodyBGTex;
uniform sampler2D _bodyHighlightTex1;
uniform sampler2D _bodyHighlightTex2;

uniform vec4 _lightColor : source_color = vec4(1.);
void fragment() {
	vec2 uv = UV;
	vec4 col = texture(_bodyBGTex, uv);
	float highlight1 = texture(_bodyHighlightTex1, uv).a;
	float highlight2 = texture(_bodyHighlightTex2, uv).a;

	vec3 lightCol = _lightColor.xyz;
	vec3 baseCol = vec3(.5,.4,.2)*2.*lightCol;
	col.xyz *= baseCol*.85;
	col.xyz = mix(col.xyz, baseCol, highlight1); 
	col.xyz = mix(col.xyz, vec3(1.), highlight2);



	float alphaMod = 1.0f;
	ALBEDO = col.xyz;
	ALPHA = col.w*alphaMod;
}
