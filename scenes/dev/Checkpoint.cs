using System.Linq;
using Godot;
public partial class Checkpoint : Area3D
{
    public Node3D SpawnPoint;
    public bool WasActivated = false;

    public override void _Ready()
    {
        SpawnPoint = (Node3D)GetChildren().First(x => x.IsInGroup("respown_point"));
        if (SpawnPoint == null)
        {
            GD.PrintErr("Checkpoint: SpawnPoint is null");
        }
    }
}
