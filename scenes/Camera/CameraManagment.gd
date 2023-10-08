extends Camera3D

class_name CameraManagment

@export var ToFollow: Node3D
@export var OffsetY: float = 2.0
@export var FloatingRate: float = 0.05

func _ready() -> void:
    if ToFollow == null:
        push_error("CameraManagment: target is null")

# Will smothly move the camera to the target
func _process(_delta: float) -> void:
    var targetPosition: Vector3 = Vector3(ToFollow.position.x, ToFollow.position.y + OffsetY, self.global_position.z)
    var deplacment: Vector3 = self.global_position.slerp(targetPosition, FloatingRate)
    self.global_position = Vector3(deplacment.x, deplacment.y, self.global_position.z)
