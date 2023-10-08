class_name WingsMouvement

var LeftWing = Enums.WingMovement.Idle
var RightWing = Enums.WingMovement.Idle
var AnimationLeftWing: AnimationPlayer
var AnimationRightWing: AnimationPlayer

var whenLeftWingFlapDown: int
var whenRightWingFlapDown: int
var deltaInMillisecondToAcceptUpBoost = 100

func Update() -> Vector2:
    LeftWing = Enums.WingMovement.Idle
    RightWing = Enums.WingMovement.Idle
    if Input.is_action_just_pressed("ui_left"):
        LeftWing = Enums.WingMovement.Up
        AnimationLeftWing.play("left_wing_up")
    if Input.is_action_just_released("ui_left"):
        LeftWing = Enums.WingMovement.Down
        AnimationLeftWing.play("left_wing_down")
        whenLeftWingFlapDown = Time.get_ticks_msec()
    if Input.is_action_just_pressed("ui_right"):
        RightWing = Enums.WingMovement.Up
        AnimationRightWing.play("right_wing_up")
    if Input.is_action_just_released("ui_right"):
        RightWing = Enums.WingMovement.Down
        AnimationRightWing.play("right_wing_down")
        whenRightWingFlapDown = Time.get_ticks_msec()

    return GetFlyingDirection()

func GetFlyingDirection() -> Vector2:
    var direction = Vector2()
    var hasUpBoost = abs(whenLeftWingFlapDown - whenRightWingFlapDown) < deltaInMillisecondToAcceptUpBoost
    if LeftWing == Enums.WingMovement.Down or RightWing == Enums.WingMovement.Down:
        direction.y = 2 if hasUpBoost else 1
    if OnlyOneWingFlap():
        direction.x = 1 if LeftWing == Enums.WingMovement.Down else -1
    return direction

func OnlyOneWingFlap() -> bool:
    return (LeftWing == Enums.WingMovement.Down and RightWing != Enums.WingMovement.Down) or (LeftWing != Enums.WingMovement.Down and RightWing == Enums.WingMovement.Down)
