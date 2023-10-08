extends AnimatedSprite3D

class_name MaterialAnimation

var sens = true

func _ready():
    play()

func _process(_delta):
    (material_override as ShaderMaterial).set_shader_parameter("albedo_texture", sprite_frames.get_frame_texture("default", frame))
