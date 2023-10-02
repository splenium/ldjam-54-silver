using Godot;

public partial class FollowPath : PathFollow3D
{
    [Export]
    public float speed = 0.04f;


    private int sens = 1;
    public override void _Ready()
    {
    }

    public override void _Process(double delta)
    {
        GD.Print(ProgressRatio, " ", sens);
        ProgressRatio += speed * (float)delta * sens;
        if (ProgressRatio >= 1)
        {
            ProgressRatio = 0.999f;
            sens = -1;
        }
        else if (ProgressRatio <= 0)
        {
            sens = 1;
        }
    }
}
