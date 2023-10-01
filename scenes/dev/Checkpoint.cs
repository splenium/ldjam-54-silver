using System.Linq;
using Godot;
public partial class Checkpoint : Area3D
{
    [Export]
    public Node3D SpawnPoint;
    public bool WasActivated = false;

    public override void _Ready()
    {
        if (SpawnPoint == null)
        {
            GD.PrintErr("Checkpoint: SpawnPoint is null");
        }
    }
}
