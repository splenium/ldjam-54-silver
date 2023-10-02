using Godot;

public partial class EndDetector : Area3D
{
    // Called when the node enters the scene tree for the first time.
    private bool alreadyTriggered = false;
    [Export]
    TextureRect credit;

    public override void _Process(double delta)
    {
        if (!alreadyTriggered && HasOverlappingBodies())
        {
            GD.Print("End");
            credit.Visible = true;
        }
    }


}
