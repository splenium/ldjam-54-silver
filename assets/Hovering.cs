using Godot;
using System;

public partial class Hovering : CsgMesh3D
{
	[Export]
	public float Speed = 1.0f;
	[Export]
	public float Amplitude = 1.0f;
	[Export]
	public float Offset = 0.0f;


	private Vector3 _origialLocalPosition;
	private float _totalTime;

	public override void _Ready()
	{
		_origialLocalPosition = this.Position;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		float t = _totalTime;
		this.Position = _origialLocalPosition + new Vector3(Mathf.Sin(t * Speed + Offset) + Mathf.Sin(t * Speed * 1.3f + Offset) * 0.5f + Mathf.Sin(t * Speed * 2.3f + Offset) * 0.25f,
			Mathf.Sin(5.0f + t * Speed + Offset) + Mathf.Sin(3.0f + t * Speed * 1.3f + Offset) * 0.5f + Mathf.Sin(1.5f + t * Speed * 2.3f + Offset) * 0.25f,
			0.0f) * Amplitude;
		_totalTime += (float)delta;

	}
}
