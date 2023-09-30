using Godot;
using System;

public partial class ShakeRotation : Node3D
{
    [Export]
    public float Speed = 1.0f;
    [Export]
    public float Amplitude = 1.0f;
    [Export]
    public float Offset = 0.0f;

    [Export]
    public Vector3 RotationAxis;


    private Vector3 _origialLocalRotation;
    private float _totalTime;

    public override void _Ready()
    {
        _origialLocalRotation = this.Rotation;

    }

    public override void _Process(double delta)
    {
        float t = _totalTime;
        this.Rotation = _origialLocalRotation +
            (Mathf.Sin(5.0f + t * Speed + Offset) + Mathf.Sin(3.0f + t * Speed * 1.3f + Offset) * 0.5f + Mathf.Sin(1.5f + t * Speed * 2.3f + Offset) * 0.25f) * RotationAxis * Amplitude;
        _totalTime += (float)delta;
    }
}
