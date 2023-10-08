extends Area3D

class_name EndDetector

var alreadyTriggered = false
@export var credit:TextureRect

func _process(_delta):
    if !alreadyTriggered and has_overlapping_bodies():
        print("End")
        credit.visible = true
