using Godot;

public partial class Level : Node3D
{

    [Export]
    public AudioStream LevelSound;
    [Export]
    public Resource NextScene;
    [Export]
    public float VolumeDb = 0f;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GameManager.PlayMusic(LevelSound, VolumeDb);
    }
}
