using Godot;
using System;

class WingsMouvement
{
    public EnumWingMovement LeftWing = EnumWingMovement.Idle;
    public EnumWingMovement RightWing = EnumWingMovement.Idle;
    public AnimationPlayer AnimationLeftWing;
    public AnimationPlayer AnimationRightWing;

    private long whenLeftWingFlapDown;
    private long whenRightWingFlapDown;
    private long deltaInMillisecondToAcceptUpBoost = 100;
    private Fly parent;

    public WingsMouvement(Fly parent)
    {
        this.parent = parent;
    }

    public Vector2 Update()
    {
        LeftWing = EnumWingMovement.Idle;
        RightWing = EnumWingMovement.Idle;
        if (Input.IsActionJustPressed("ui_left"))
        {
            LeftWing = EnumWingMovement.Up;
            AnimationLeftWing.Play("left_wing_up");
        }
        if (Input.IsActionJustReleased("ui_left"))
        {
            LeftWing = EnumWingMovement.Down;
            AnimationLeftWing.Play("left_wing_down");
            whenLeftWingFlapDown = (long)Time.GetTicksMsec();
        }
        if (Input.IsActionJustPressed("ui_right"))
        {
            RightWing = EnumWingMovement.Up;
            AnimationRightWing.Play("right_wing_up");
        }
        if (Input.IsActionJustReleased("ui_right"))
        {
            RightWing = EnumWingMovement.Down;
            AnimationRightWing.Play("right_wing_down");
            whenRightWingFlapDown = (long)Time.GetTicksMsec();
        }

        return GetFlyingDirection();
    }

    private Vector2 GetFlyingDirection()
    {
        var direction = new Vector2();
        var hasUpBoost = Math.Abs(whenLeftWingFlapDown - whenRightWingFlapDown) < deltaInMillisecondToAcceptUpBoost;
        if (LeftWing == EnumWingMovement.Down || RightWing == EnumWingMovement.Down)
        {
            direction.Y = hasUpBoost ? 2 : 1;
            if (hasUpBoost)
            {
                parent.PlayBoostSound();
            }
        }
        if (OnlyOneWingFlap())
        {
            direction.X = LeftWing == EnumWingMovement.Down ? 1 : -1;
        }
        return direction;
    }

    private bool OnlyOneWingFlap() =>
        (LeftWing == EnumWingMovement.Down && RightWing != EnumWingMovement.Down) ||
        (LeftWing != EnumWingMovement.Down && RightWing == EnumWingMovement.Down);
}