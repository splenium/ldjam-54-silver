using Godot;
using LudumDare54Silver.scenes.PlayerController.Power;
using System;

public partial class Fly : Power
{
    [Export]
    public float Speed = 2.0f;
    [Export]
    public float JumpVelocity = 6f;

    [Export]
    public AnimationPlayer AnimationLeftWing;
    [Export]
    public AnimationPlayer AnimationRightWing;

    private WingsMouvement wingsPosition = new WingsMouvement();

    public override void _Ready()
    {
        base._Ready();
        Visible = false;
        AnimationLeftWing.Play("left_wing_down");
        AnimationRightWing.Play("right_wing_down");
        wingsPosition.AnimationLeftWing = AnimationLeftWing;
        wingsPosition.AnimationRightWing = AnimationRightWing;
    }


    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle() * 2;

    public override void MoveCharacter(CharacterBody3D character, double delta)
    {
        Vector3 velocity = character.Velocity;

        float gravityForce = gravity * (float)delta;

        // Add the gravity.
        if (!character.IsOnFloor())
            velocity.Y -= gravityForce;

        velocity = WaterBehavior(gravityForce, velocity, delta);

        var direction = wingsPosition.Update();


        if (direction.Y != 0)
        {
            velocity.Y = direction.Y * JumpVelocity;
        }

        if (direction.X != 0)
        {
            velocity.X += direction.X * Speed;
            velocity.X = Mathf.Clamp(velocity.X, -3 * Speed, 3 * Speed);
        }
        else
        {
            velocity.X = Mathf.MoveToward(character.Velocity.X, 0, Speed * 2 * (float)delta);
        }

        character.Velocity = velocity;
        character.MoveAndSlide();

        SetRaKoonAvatarAnimation(velocity);
    }
}

class WingsMouvement
{
    public WingMovement LeftWing = WingMovement.Idle;
    public WingMovement RightWing = WingMovement.Idle;
    public AnimationPlayer AnimationLeftWing;
    public AnimationPlayer AnimationRightWing;

    private long whenLeftWingFlapDown;
    private long whenRightWingFlapDown;
    private long deltaInMillisecondToAcceptUpBoost = 100;

    public Vector2 Update()
    {
        LeftWing = WingMovement.Idle;
        RightWing = WingMovement.Idle;
        if (Input.IsActionJustPressed("ui_left"))
        {
            LeftWing = WingMovement.Up;
            AnimationLeftWing.Play("left_wing_up");
        }
        if (Input.IsActionJustReleased("ui_left"))
        {
            LeftWing = WingMovement.Down;
            AnimationLeftWing.Play("left_wing_down");
            whenLeftWingFlapDown = (long)Time.GetTicksMsec();
        }
        if (Input.IsActionJustPressed("ui_right"))
        {
            RightWing = WingMovement.Up;
            AnimationRightWing.Play("right_wing_up");
        }
        if (Input.IsActionJustReleased("ui_right"))
        {
            RightWing = WingMovement.Down;
            AnimationRightWing.Play("right_wing_down");
            whenRightWingFlapDown = (long)Time.GetTicksMsec();
        }

        return GetFlyingDirection();
    }

    private Vector2 GetFlyingDirection()
    {
        var direction = new Vector2();
        var hasUpBoost = Math.Abs(whenLeftWingFlapDown - whenRightWingFlapDown) < deltaInMillisecondToAcceptUpBoost;
        if (LeftWing == WingMovement.Down || RightWing == WingMovement.Down)
        {
            direction.Y = hasUpBoost ? 2 : 1;
        }
        if (OnlyOneWingFlap())
        {
            direction.X = LeftWing == WingMovement.Down ? 1 : -1;
        }
        return direction;
    }

    private bool OnlyOneWingFlap() =>
        (LeftWing == WingMovement.Down && RightWing != WingMovement.Down) ||
        (LeftWing != WingMovement.Down && RightWing == WingMovement.Down);
}

enum WingMovement
{
    Idle,
    Down,
    Up
}