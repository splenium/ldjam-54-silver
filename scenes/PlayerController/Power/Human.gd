extends Power

class_name Human

@export var Speed = 6
@export var JumpVelocity = 7
var Gravity = ProjectSettings.get_setting("physics/3d/default_gravity")

func MoveCharacter(character: CharacterBody3D, delta: float) -> void:
    var velocity = character.velocity
    var gravityForce = Gravity * delta

    if not character.is_on_floor():
        velocity.y -= gravityForce

    velocity = WaterBehavior(gravityForce, velocity, delta)

    if Input.is_action_just_pressed("ui_up") and (character.is_on_floor() or InWater()):
        velocity.y = JumpVelocity

    var inputDir = Input.get_vector("ui_left", "ui_right", "ui_up", "ui_down")
    var direction = (transform.basis * Vector3(inputDir.x, 0, inputDir.y)).normalized()
    if direction != Vector3.ZERO:
        velocity.x = direction.x * Speed
    else:
        velocity.x = move_toward(character.velocity.x, 0, Speed)

    raKoonAvatar.SetFace((randi() % 5) as Enums.EFaceState)

    character.velocity = velocity
    character.move_and_slide()

    SetRaKoonAvatarAnimation(velocity)
