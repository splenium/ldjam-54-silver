using System;
using Godot;
using LudumDare54Silver.scenes.PlayerController.Power;

public partial class Fly : Node3D, Power
{
    [Export]
    public float Speed = 20.0f;
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

    public void MoveCharacter(CharacterBody3D character, double delta)
    {
        Vector3 velocity = character.Velocity;

        // Add the gravity.
        if (!character.IsOnFloor())
            velocity.Y -= gravity * (float)delta;

        var direction = wingsPosition.Update();


        if (direction.Y != 0)
        {
            velocity.Y = direction.Y * JumpVelocity;
        }

        if (direction.X != 0)
        {
            velocity.X = direction.X * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(character.Velocity.X, 0, Speed / 2);
        }

        character.Velocity = velocity;
        character.MoveAndSlide();
    }

    public void Init(CharacterBody3D c)
    {
        Visible = true;
    }

    public void Exit(CharacterBody3D c)
    {
        Visible = false;
    }

    public bool CanChange(CharacterBody3D c)
    {
        return true;
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
    private long deltaInMillisecondToAcceptUpBoost = 200;

    public Vector2 Update()
    {
        var isFlapping = false;
        LeftWing = WingMovement.Idle;
        RightWing = WingMovement.Idle;
        if (Input.IsActionJustPressed("ui_left"))
        {
            LeftWing = WingMovement.Up;
            AnimationLeftWing.Play("left_wing_up");
            whenLeftWingFlapDown = 0;
        }
        if (Input.IsActionJustReleased("ui_left"))
        {
            isFlapping = true;
            LeftWing = WingMovement.Down;
            AnimationLeftWing.Play("left_wing_down");
            whenLeftWingFlapDown = (long)Time.GetTicksMsec();
        }
        if (Input.IsActionJustPressed("ui_right"))
        {
            RightWing = WingMovement.Up;
            AnimationRightWing.Play("right_wing_up");
            whenRightWingFlapDown = 0;
        }
        if (Input.IsActionJustReleased("ui_right"))
        {
            isFlapping = true;
            RightWing = WingMovement.Down;
            AnimationRightWing.Play("right_wing_down");
            whenRightWingFlapDown = (long)Time.GetTicksMsec();
        }

        return isFlapping ? GetFlyingDirection() : new Vector2();
    }

    private Vector2 GetFlyingDirection()
    {
        var direction = new Vector2();
        var hasUpBoost = Math.Abs(whenLeftWingFlapDown - whenRightWingFlapDown) < deltaInMillisecondToAcceptUpBoost;
        if (LeftWing == WingMovement.Down || RightWing == WingMovement.Down)
        {
            direction.Y = hasUpBoost ? 2 : 1;
        }
        if (!hasUpBoost && OnlyOneWingFlap())
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