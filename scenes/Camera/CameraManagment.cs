using Godot;

public partial class CameraManagment : Camera3D
{
    [Export]
    public Node3D ToFollow;
    [Export]
    public float OffsetY = 2f;
    [Export]
    public float FloatingRate = 0.05f;

    public override void _Ready()
    {
        if (ToFollow == null)
        {
            GD.PrintErr("CameraManagment: target is null");
        }
    }

    // Will smothly move the camera to the target
    public override void _Process(double delta)
    {
        Vector3 targetPosition = new Vector3(ToFollow.Position.X, ToFollow.Position.Y + OffsetY, this.GlobalPosition.Z);
        Vector3 deplacment = this.GlobalPosition.Slerp(targetPosition, FloatingRate);
        this.GlobalPosition = new Vector3(deplacment.X, deplacment.Y, this.GlobalPosition.Z);
    }
}
