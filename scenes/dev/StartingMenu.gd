extends Control

class_name StartingMenu

@export var StartingScene: PackedScene
@export var CreditsControl: Control
@export var Box: BoxContainer

func _on_start_button_pressed():
	get_tree().change_scene_to_file(StartingScene.resource_path)

func _on_credits_button_pressed():
	CreditsControl.visible = true
	Box.visible = false

func _on_back_button_pressed():
	CreditsControl.visible = false
	Box.visible = true
