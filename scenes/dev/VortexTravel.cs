using Godot;
using System;

public partial class VortexTravel : Node3D
{
    [Export]
    public float TravelPercent; // From 0 to 1

    [Export]
    public RaKoonAvatar RaKoonAvatar;


    private Vector3 _originalRotation;
    private Vector3 _originalPosition;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _originalRotation = new Vector3(0, 0,-20);// RaKoonAvatar.Rotation;
        _originalPosition = new Vector3(-1, 0,0);
    }

    public override void _Process(double delta)
    {
        RaKoonAvatar.LightColor = new Color(0.97f, 0.13f, 0.62f);
        RaKoonAvatar.IsLeft = false;
        var moveObject = (RaKoonAvatar.GetParent() as Node3D);
        moveObject.Rotation = _originalRotation.Lerp(new Vector3(0, 0, 10), TravelPercent);
        moveObject.Position = _originalPosition.Lerp(new Vector3(1,0,0), TravelPercent);
    }

}
