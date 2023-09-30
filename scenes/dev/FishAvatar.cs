using Godot;
using System;
using System.Collections.Generic;

public partial class FishAvatar : Node3D
{
    [Export]
    public Color LightColor;
	[Export]
	public CsgMesh3D[] Objects;

	public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{
        foreach (var child in Objects)
        {
            child.Material.Set("albedo_color", LightColor);
        }
    }
}
