extends Area3D

class_name Checkpoint

@export var SpawnPoint: Node3D
var WasActivated = false

func _ready():
    if SpawnPoint == null:
        push_error("Checkpoint: SpawnPoint is null")
