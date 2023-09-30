shader_type spatial;
render_mode unshaded;

uniform float _saturation  : hint_range(0, 1);
uniform vec3 _color : source_color = vec3(1.0);
uniform sampler2D _mainTex;

#define sat(a) clamp(a, 0., 1.)

varying vec3 world_position;

void vertex()
{
    world_position = (MODEL_MATRIX * vec4(VERTEX, 1.0)).xyz;
}

void fragment() {
	vec2 uv = UV;//world_position.xy;
	vec3 col = vec3(0., 0., 0.);
	
	col = texture(_mainTex, uv).xyz;
	col = mix(col, vec3(1.)*(col.x+col.y+col.z)/3., 1.-_saturation);
	col *= _color;
	ALBEDO = col;
}
