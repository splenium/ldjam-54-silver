extends Node3D

class_name Level

@export var LevelSound: AudioStream
@export var DefaultCheckpoint: Checkpoint
@export var VolumeDb: float = 0.0
@export var NextLevelNumber: int = 1

func _ready() -> void:
    GameManager.PlayMusic(LevelSound, VolumeDb)
    GameManager.InitializeCheckpoint(DefaultCheckpoint)