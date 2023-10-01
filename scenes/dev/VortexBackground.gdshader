shader_type spatial;
render_mode unshaded;

uniform sampler2D iChannel0;

void fragment() {
	vec2 uv = UV;
	    uv *= .2;
    float t= TIME*.2;
    vec3 col = vec3(0.);
    col = texture(iChannel0, uv*vec2(.01,1.)+vec2(t*.01, 0.)).x
    *vec3(1.000,0.000,0.584);
    col += texture(iChannel0, uv*vec2(.01,1.)*.5+vec2(t*.01, 0.)).x*
    vec3(1.,.7,.2)
    *texture(iChannel0, (uv*vec2(.01,1.)*.5+vec2(t*.01, 0.))*.1).x;
	ALBEDO = col;
}
