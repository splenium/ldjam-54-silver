shader_type spatial;
render_mode unshaded, blend_mix;

uniform sampler2D _headBGTex;
uniform sampler2D _headOutlineTex;
uniform sampler2D _headHighLightTex;
uniform vec4 _lightColor : source_color = vec4(1.);
uniform float _ghostness;

void fragment() {
	vec4 col = texture(_headBGTex, UV);
	float highlight = texture(_headHighLightTex, UV).a;
	float outline = texture(_headOutlineTex, UV).a;
	vec3 lightCol = _lightColor.xyz;
	vec3 baseCol = vec3(0.22f, 0.2f, 0.2f)*1.8*_lightColor.xyz;	
	col.xyz *= mix(baseCol, vec3(1.)*(baseCol.x+baseCol.y+baseCol.z)/3., .5);
	col.xyz = mix(col.xyz, baseCol, highlight);
	col.xyz = mix(col.xyz, vec3(0.), outline);
	bool isGhost = _ghostness > 0.5;
	float alphaMod = 1.0f;
	if (isGhost)
	{
		col.xyz = 1.-vec3(1.)*(col.x+col.y+col.z)/3.;
		col.xyz = pow(col.xyz, vec3(2.));
		alphaMod = 0.7;
		col.xyz *= mix(vec3(0.5,.7,.6), lightCol, .5);
	}
	ALBEDO = col.xyz;
	ALPHA = max(col.w, outline)*alphaMod;
}
