using Godot;

public partial class MaterialAnimation : AnimatedSprite3D
{
    private bool sens = true;
    public override void _Ready()
    {
        base._Ready();
        Play();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        (MaterialOverride as ShaderMaterial).SetShaderParameter("albedo_texture", this.SpriteFrames.GetFrameTexture("default", Frame));
    }
}
