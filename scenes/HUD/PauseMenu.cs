using Godot;

public partial class PauseMenu : Control
{
    public void TogglePause()
    {
        Visible = !Visible;
        GetTree().Paused = !GetTree().Paused;
    }
}
