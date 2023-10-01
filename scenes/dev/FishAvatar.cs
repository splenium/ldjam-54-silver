using Godot;

public partial class FishAvatar : Area3D
{
    [Export]
    public Color LightColor;
	[Export]
	public CsgMesh3D[] Objects;
    [Export]
    public AnimationPlayer AttackAnimation;

	public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{
        foreach (var child in Objects)
        {
            child.Material.Set("albedo_color", LightColor);
        }
        if (HasOverlappingBodies())
        {
            var player = (CharacterController)GetOverlappingBodies()[0];
            if (player != null)
            {
                AttackAnimation.Play("attack");
                player.TakeDamage(50);
            }
        }
    }

    public void FacingDirection(bool isRight)
    {
        var scaleX = Mathf.Abs(Scale.X);
        var scaleY = Scale.Y;
        var scaleZ = Scale.Z;

        Scale = new Vector3(isRight ? scaleX : -scaleX, scaleY, scaleZ);
    }
}
