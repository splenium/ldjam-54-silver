using Godot;
using System;

public partial class BodyMouvement : AnimationPlayer
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        Play("up_and_down");
    }
}
