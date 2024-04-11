shader_type spatial;
render_mode unshaded;

// MIT License. Made by Leo Peltola
// Inspired by https://threejs.org/examples/webgl_postprocessing_pixel.html

uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, filter_linear_mipmap;

void fragment() {
	vec2 grid_uv = SCREEN_UV;
	vec4 original_color = texture(SCREEN_TEXTURE, grid_uv).rgba;
	vec3 final = original_color.rgb;
	//final = round(final*50.0f) / 50.0f;
	
	ALBEDO = final;
	ALPHA = original_color.a;
}