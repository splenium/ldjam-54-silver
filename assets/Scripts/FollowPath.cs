using Godot;

public partial class FollowPath : PathFollow3D
{
    [Export]
    public float speed = 0.04f;
    [Export]
    public bool PlayOnFlyModeUnlock = true;
    [Export]
    public bool Reverse = true;


    private int sens = 1;
    private float progression = 1;
    private Vector3 rotation;

    public override void _Ready()
    {
        progression = ProgressRatio;
        rotation = RotationDegrees;

    }

    public override void _Process(double delta)
    {
        if (PlayOnFlyModeUnlock && !GameManager.MyPlayer.IsPowerFlyUnlock)
            return;
        RotationDegrees = rotation;
        progression += speed * (float)delta * sens;
        ProgressRatio = progression;

        if (Reverse)
        {
            if (progression >= 1)
            {
                sens = -1;
            }
            else if (ProgressRatio <= 0)
            {
                sens = 1;
            }
        }
        else if (progression >= 1)
        {
            progression = 0;
        }
    }

    public void Enable()
    {
        PlayOnFlyModeUnlock = true;
    }

    public void Disable()
    {
        PlayOnFlyModeUnlock = false;
    }
}
