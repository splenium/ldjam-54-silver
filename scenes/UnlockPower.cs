using Godot;
public partial class UnlockPower : Area3D
{
    [Export]
    public PowerEnum power;
    public override void _Process(double delta)
    {
        if (HasOverlappingBodies())
        {
            var player = (CharacterController)GetOverlappingBodies()[0];
            if (player != null)
            {
                player.UnlockPower(power);
                QueueFree();
            }
        }
    }
}
