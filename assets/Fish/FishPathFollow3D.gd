extends PathFollow3D

class_name FishPathFollow3D

@export
var fish_avatar: FishAvatar
@export
var speed: float = 0.04

var previous_position: Vector3

func _ready():
    previous_position = global_position

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
    progress_ratio += speed * delta
    if (progress_ratio >= 1):
        progress_ratio = 0
    var is_going_right = global_position.x > previous_position.x
    fish_avatar.facing_direction(is_going_right)
    previous_position = global_position
