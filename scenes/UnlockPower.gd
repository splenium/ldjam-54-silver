extends Area3D

class_name UnlockPower

@export var power: Enums.Power
@export var GetPowerSound: AudioStream

func _process(_delta: float) -> void:
    if has_overlapping_bodies():
        var player = get_overlapping_bodies()[0] as CharacterController
        if player != null:
            player.UnlockPower(power)
            GameManager.PlaySoundEffect(GetPowerSound)
            queue_free()
