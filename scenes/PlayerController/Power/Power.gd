extends Node3D

class_name Power

@export var WaterDetector: Area3D
@export var ArchimedesScale: float = 2.0
@export var ContactWaterBreak: float = 0.2
@export var raKoonAvatar: RaKoonAvatar
@export var LightColor: Color
@export var PowerLabel: String = "RaKoon"
var wasInWater: bool = false

func _ready() -> void:
    visible = false
    process_mode = PROCESS_MODE_DISABLED
    if raKoonAvatar == null:
        push_error("Power: raKoonAvatar is null")

func _process(_delta: float) -> void:
    pass

func Init(_character: CharacterBody3D) -> void:
    process_mode = PROCESS_MODE_INHERIT
    visible = true
    raKoonAvatar.LightColor = LightColor

func MoveCharacter(_character: CharacterBody3D, _delta: float) -> void:
    pass

func Exit(_add_named_global_constantcharacter: CharacterBody3D) -> void:
    visible = false
    process_mode = PROCESS_MODE_DISABLED

func CanChange(_character: CharacterBody3D) -> bool:
    return true

func InWater() -> bool:
    return WaterDetector.has_overlapping_areas() or WaterDetector.has_overlapping_bodies()

func SetRaKoonAvatarAnimation(velocity: Vector3) -> void:
    if velocity.x > 0:
        raKoonAvatar.IsLeft = false
    elif velocity.x < 0:
        raKoonAvatar.IsLeft = true
    raKoonAvatar.IsMoving = not velocity.is_zero_approx()

func WaterBehavior(gravityForce: float, velocity: Vector3, _delta: float) -> Vector3:
    if InWater():
        if not wasInWater:
            velocity.y = velocity.y * (1.0 - ContactWaterBreak)
        wasInWater = true
        velocity.y += gravityForce * ArchimedesScale
    elif wasInWater:
        wasInWater = false
        velocity.y -= gravityForce * 0.8
    return velocity
