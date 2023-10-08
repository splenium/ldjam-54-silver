extends Power

class_name Fish

@export var MaxSpeed = 6
@export var Acceleration = 10
@export var GravityScaleUnderwater = 0.05

# Get the gravity from the project settings to be synced with RigidBody nodes.
var Gravity = ProjectSettings.get_setting("physics/3d/default_gravity")

func Init(c):
    super.Init(c)
    raKoonAvatar.IsTriton = true

func Exit(c):
    super.Exit(c)
    raKoonAvatar.IsTriton = false

func MoveCharacter(character: CharacterBody3D, delta: float):
    var velocity = character.velocity

    if InWater():
        var gravity = Gravity * GravityScaleUnderwater * delta
        velocity.y -= gravity

        # Add the gravity.

        # Get the input direction and handle the movement/deceleration.
        # As good practice, you should replace UI actions with custom gameplay actions.
        var inputDir = Input.get_vector("ui_left", "ui_right", "ui_down", "ui_up")
        var direction = (transform.basis * Vector3(inputDir.x, inputDir.y, 0)).normalized()
        if direction.x != Vector3.ZERO.x:
            velocity.x += direction.x * Acceleration * delta
        else:
            velocity.x = move_toward(character.velocity.x, 0, Acceleration * delta)

        if direction.y != Vector3.ZERO.y:
            velocity.y += direction.y * Acceleration * delta + gravity

        if abs(velocity.y) > MaxSpeed:
            if velocity.y > 0:
                velocity.y = MaxSpeed
            else:
                velocity.y = -MaxSpeed
        if abs(velocity.x) > MaxSpeed:
            if velocity.x > 0:
                velocity.x = MaxSpeed
            else:
                velocity.x = -MaxSpeed
    elif not character.is_on_floor():
        velocity.y -= Gravity * delta
    else:
        velocity.x = move_toward(character.velocity.x, 0, MaxSpeed)

    character.velocity = velocity
    character.move_and_slide()
    SetRaKoonAvatarAnimation(velocity)
