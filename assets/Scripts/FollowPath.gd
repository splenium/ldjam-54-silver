extends PathFollow3D

class_name FollowPath

@export var speed = 0.04
@export var PlayOnFlyModeUnlock = true
@export var Reverse = true

var sens = 1
var progression = 1
var lock_rotation = Vector3()

func _ready():
    progression = progress_ratio
    lock_rotation = rotation_degrees

func _process(delta):
    if PlayOnFlyModeUnlock and not GameManager.MyPlayer.IsPowerFlyUnlock:
        return
    rotation_degrees = lock_rotation
    progression += speed * delta * sens
    progress_ratio = progression

    if Reverse:
        if progression >= 1:
            sens = -1
        elif progress_ratio <= 0:
            sens = 1
    elif progression >= 1:
        progression = 0

func Enable():
    PlayOnFlyModeUnlock = true

func Disable():
    PlayOnFlyModeUnlock = false
