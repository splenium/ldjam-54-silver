using Godot;
using System;

public partial class FishPathFollow3D : PathFollow3D
{
    [Export]
    public FishAvatar FishAvatar;

    [Export]
    public float speed = 0.04f;

    private Vector3 previousPosition;
    public override void _Ready()
    {
        previousPosition = GlobalPosition;
    }

    public override void _Process(double delta)
    {
        ProgressRatio += speed * (float)delta;
        if (ProgressRatio >= 1)
            ProgressRatio = 0;

        var isGoingRight = GlobalPosition.X > previousPosition.X;
        FishAvatar.FacingDirection(isGoingRight);
        previousPosition = GlobalPosition;
    }
}
