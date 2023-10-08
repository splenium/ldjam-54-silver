extends Node3D

class_name ShakeRotation

@export var Speed = 1.0
@export var Amplitude = 1.0
@export var Offset = 0.0

@export var RotationAxis: Vector3

var _origialLocalRotation: Vector3
var _totalTime: float

func _ready():
    _origialLocalRotation = self.rotation

func _process(delta):
    var t = _totalTime
    self.rotation = _origialLocalRotation + (sin(5.0 + t * Speed + Offset) + sin(3.0 + t * Speed * 1.3 + Offset) * 0.5 + sin(1.5 + t * Speed * 2.3 + Offset) * 0.25) * RotationAxis * Amplitude
    _totalTime += delta
