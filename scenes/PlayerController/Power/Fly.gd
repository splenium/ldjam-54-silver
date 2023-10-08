extends Power

class_name Fly

@export var Speed = 2.0
@export var JumpVelocity = 6.0

@export var AnimationLeftWing: AnimationPlayer
@export var AnimationRightWing: AnimationPlayer

@export var WingRight: CSGMesh3D
@export var WingLeft: CSGMesh3D

@export var BoostSound: AudioStreamPlayer
@export var BoostSounds: Array[AudioStream]

var wingsPosition = WingsMouvement.new()

func _ready():
    super._ready()
    visible = false
    AnimationLeftWing.play("left_wing_down")
    AnimationRightWing.play("right_wing_down")
    wingsPosition.AnimationLeftWing = AnimationLeftWing
    wingsPosition.AnimationRightWing = AnimationRightWing

func _process(delta):
    super._process(delta)
    WingRight.material.set("albedo_color", LightColor)
    WingLeft.material.set("albedo_color", LightColor)

# Get the gravity from the project settings to be synced with RigidBody nodes.
var gravity = ProjectSettings.get_setting("physics/3d/default_gravity")

func MoveCharacter(character: CharacterBody3D, delta):
    var velocity = character.velocity

    var gravityForce = gravity * delta * 2

    # Add the gravity.
    if not character.is_on_floor():
        velocity.y -= gravityForce

    velocity = WaterBehavior(gravityForce, velocity, delta)

    var direction = wingsPosition.Update()


    if direction.y != 0:
        velocity.y = direction.y * JumpVelocity

    if direction.x != 0:
        velocity.x += direction.x * Speed
        velocity.x = clamp(velocity.x, -3 * Speed, 3 * Speed)
    else:
        velocity.x = move_toward(character.velocity.x, 0, Speed * 2 * delta)
    
    character.velocity = velocity
    character.move_and_slide()

    SetRaKoonAvatarAnimation(velocity)
