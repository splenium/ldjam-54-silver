shader_type spatial;
render_mode unshaded, blend_mix;

uniform sampler2D _bodyBGTex;
uniform sampler2D _bodyHighlightTex1;
uniform sampler2D _bodyHighlightTex2;

uniform vec4 _lightColor : source_color = vec4(1.);
uniform float _ghostness;
#define sat(a) clamp(a, 0., 1.)
void fragment() {
	vec4 col = texture(_bodyBGTex, UV);
	float highlight1 = texture(_bodyHighlightTex1, UV).a;
	float highlight2 = texture(_bodyHighlightTex2, UV).a;

	vec3 lightCol = _lightColor.xyz;
	vec3 baseCol = vec3(.5,.4,.2)*2.*lightCol;
	col.xyz *= baseCol*.85;
	col.xyz = mix(col.xyz, baseCol, highlight1); 
	col.xyz = mix(col.xyz, vec3(1.), highlight2);
	bool isGhost = _ghostness > 0.5;
	float ghostLight = length(UV-.5)-0.2;
	if (isGhost)
		col.w += (1.-sat(ghostLight/.3))*.5;
	float alphaMod = 1.0f;
	if (isGhost)
	{
		col.xyz = 1.-vec3(1.)*(col.x+col.y+col.z)/3.;
		col.xyz = pow(col.xyz, vec3(.5));
		alphaMod = 0.5;
		col.xyz *= mix(vec3(0.5,.7,.6), lightCol, .5);
	}
	ALBEDO = col.xyz;
	ALPHA = col.w*alphaMod;
}
