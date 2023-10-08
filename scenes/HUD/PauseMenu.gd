extends Control

class_name PauseMenu

func TogglePause():
    visible = not visible
    get_tree().paused = not get_tree().paused
