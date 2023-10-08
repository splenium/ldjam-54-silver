extends Area3D

class_name FishAvatar

@export
var light_color: Color
@export
var objects: Array[CSGMesh3D]
@export
var attack_animation: AnimationPlayer

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta):
    for child in objects:
        child.material.set("albedo_color", light_color)
    if (has_overlapping_bodies()):
        var player = get_overlapping_bodies()[0] as CharacterController
        if (player != null):
            attack_animation.play("attack")
            player.TakeDamage(50)

func facing_direction(is_right: bool):
    var scaleX = abs(scale.x)
    var scaleY = scale.y
    var scaleZ = scale.z
    scale = Vector3(scaleX if is_right else -scaleX, scaleY, scaleZ)
