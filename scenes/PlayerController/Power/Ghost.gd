extends Power

class_name Ghost

@export var Speed = 4.0
@export var JumpVelocity = 4.5
@export var GravityScale = 0.5

@export var Detector: Area3D

# Get the gravity from the project settings to be synced with RigidBody nodes.
var Gravity = ProjectSettings.get_setting("physics/3d/default_gravity")

func _ready():
    super._ready()
    if Detector == null:
        push_error("Ghost: Detector is null")

func MoveCharacter(character: CharacterBody3D, delta: float) -> void:
    var velocity = character.velocity
    var gravityForce = Gravity * delta

    # Add the gravity.
    if not character.is_on_floor():
        velocity.y -= gravityForce

    velocity = WaterBehavior(gravityForce, velocity, delta)

    # Handle Jump.
    if Input.is_action_just_pressed("ui_up") and (character.is_on_floor() or InWater()):
        velocity.y = JumpVelocity

    # Get the input direction and handle the movement/deceleration.
    # As good practice, you should replace UI actions with custom gameplay actions.
    var inputDir = Input.get_vector("ui_left", "ui_right", "ui_up", "ui_down")
    var direction = (transform.basis * Vector3(inputDir.x, 0, inputDir.y)).normalized()
    if direction != Vector3.ZERO:
        velocity.x = direction.x * Speed
    else:
        velocity.x = move_toward(character.velocity.x, 0, Speed)

    character.velocity = velocity
    character.move_and_slide()

    SetRaKoonAvatarAnimation(velocity)

func Init(c: CharacterBody3D) -> void:
    super.Init(c)
    raKoonAvatar.IsGhost = true
    c.set_collision_mask_bit(2, false)

func Exit(c: CharacterBody3D) -> void:
    super.Exit(c)
    raKoonAvatar.IsGhost = false
    c.set_collision_mask_bit(2, true)

func CanChange(_c: CharacterBody3D) -> bool:
    return IsCollidingObject()

func IsCollidingObject() -> bool:
    return not Detector.has_overlapping_bodies()
