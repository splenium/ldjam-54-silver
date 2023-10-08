extends CSGMesh3D

class_name Hovering

@export var Speed = 1.0
@export var Amplitude = 1.0
@export var Offset = 0.0

var _origialLocalPosition: Vector3
var _totalTime: float

func _ready():
    _origialLocalPosition = self.position

func _process(delta: float) -> void:
    var t: float = _totalTime
    self.position = _origialLocalPosition + Vector3(sin(t * Speed + Offset) + sin(t * Speed * 1.3 + Offset) * 0.5 + sin(t * Speed * 2.3 + Offset) * 0.25,
        sin(5.0 + t * Speed + Offset) + sin(3.0 + t * Speed * 1.3 + Offset) * 0.5 + sin(1.5 + t * Speed * 2.3 + Offset) * 0.25,
        0.0) * Amplitude
    _totalTime += delta
