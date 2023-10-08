extends Node3D

class_name VortexTravel

var TravelPercent: float # From 0 to 1

@export var NextScenePath: String

@export var RaKoonAvatar: RaKoonAvatar

@export var animationStarted: bool

var _originalRotation: Vector3
var _originalPosition: Vector3

# Called when the node enters the scene tree for the first time.
func _ready():
    _originalRotation = Vector3(0, 0, -20) # RaKoonAvatar.rotation
    _originalPosition = Vector3(-1, 0, 0)

func _process(_delta: float) -> void:
    RaKoonAvatar.LightColor = Color(0.97, 0.13, 0.62)
    RaKoonAvatar.IsLeft = false
    var moveObject = RaKoonAvatar.get_parent() as Node3D
    moveObject.rotation = _originalRotation.lerp(Vector3(0, 0, 10), TravelPercent)
    moveObject.position = _originalPosition.lerp(Vector3(1, 0, 0), TravelPercent)
    if not animationStarted:
        start_anim()

func start_anim() -> void:
    animationStarted = true
    print("Start travel anim...")
    var delay = 1.0 / 30.0
    var duration = 5.0
    var time = 0.0
    while time < duration:
        time += delay
        TravelPercent = time / duration
        await get_tree().create_timer(delay).timeout
    print("Travel anim finished, loading scene...", GameManager.NextScene)
    get_tree().change_scene_to_file(GameManager.NextScene)
