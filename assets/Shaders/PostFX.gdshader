shader_type canvas_item;


uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_nearest;
uniform sampler2D _noiseTex;
uniform float _vignetteRadius  : hint_range(0,1);
uniform float _vignetteSharp  : hint_range(0,1);
uniform float _vignetteOpacity : hint_range(0,1);

uniform float _bloomRadius : hint_range(0,1.);
uniform float _bloomPow : hint_range(0,5);
uniform float _bloomOpacity : hint_range(0,1);

#define sat(a) clamp(a, 0., 1.)

// By Morgan McGuire @morgan3d, http://graphicscodex.com
float hash21(vec2 p) { return fract(1e4 * sin(17.0 * p.x + p.y * 0.1) * (0.1 + abs(sin(p.y * 13.0 + p.x)))); }

float hash11(vec2 uv, float seed)
{
	seed = seed+texture(_noiseTex, uv).x;//+TIME;
	return fract(sin(seed*123.456)*123.456);
}

vec3 bloom(vec2 uv)
{
	vec2 off = (vec2(hash11(uv, 0), hash11(uv, 1)))-.5;
	vec3 col = textureLod(screen_texture, 
	(uv+off*_bloomRadius), 1.0).xyz*(1.-sat(length(off)));
	return col;
}

#define pow2(x) (x * x)
#define pi 3.1415
const int samples = 20;
const float sigma = float(65) * 0.25;

float gaussian(vec2 i) {
    return 1.0 / (2.0 * pi * pow2(sigma)) * exp(-((pow2(i.x) + pow2(i.y)) / (2.0 * pow2(sigma))));
}

#define rot(a) mat2(vec2(cos(a), -sin(a)), vec2(sin(a), cos(a)));

vec3 blur(vec2 uv, vec2 scale) {
    vec3 col = vec3(0.0);
    float accum = 0.0;
    float weight = 0.0;
    vec2 offset = vec2(0.0);
    
    for (int x = -samples / 2; x < samples / 2; ++x) {
        for (int y = -samples / 2; y < samples / 2; ++y) {
            offset = vec2(float(x), float(y))*5.0*_bloomRadius;
            offset *= rot(.5*float(x));
            weight = gaussian(offset);
            col += textureLod(screen_texture, uv + scale * offset, 0.).rgb * weight;
            accum += weight;
        }
    }
    
    return col / accum;
}

void fragment() {
		vec4 col = textureLod(screen_texture, SCREEN_UV, 0.0);
		
		vec3 bloomAcc = vec3(0.);
		/*for (int i = 0; i < 16; ++i)
		{
			bloomAcc += pow(bloom(SCREEN_UV), vec3(_bloomPow));			
		}*/
		bloomAcc =pow(blur(SCREEN_UV, SCREEN_PIXEL_SIZE), vec3(_bloomPow));
		col.xyz += bloomAcc*_bloomOpacity;
		// Vignette
		float vignette = length(SCREEN_UV-.5)-_vignetteRadius;
		float maskVignette = sat(vignette*mix(0.,100., _vignetteSharp));
		col = mix(col, vec4(0.), maskVignette*_vignetteOpacity);
        COLOR = vec4(col.xyz, 1.0);
}