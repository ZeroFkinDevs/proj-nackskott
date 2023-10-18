shader_type spatial;
render_mode unshaded;

// MIT License. Made by Leo Peltola
// Inspired by https://threejs.org/examples/webgl_postprocessing_pixel.html

uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, filter_linear_mipmap;

uniform int amount = 40;

void fragment() {
	vec2 amount2 = vec2(float(amount)*(16.0f/9.0f), float(amount));
	vec2 grid_uv = round(SCREEN_UV * amount2) / amount2;
	vec3 original_color = texture(SCREEN_TEXTURE, grid_uv).rgb;
	vec3 final = original_color;
	
	ALBEDO = final;
}