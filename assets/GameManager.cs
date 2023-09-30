using Godot;
public partial class GameManager : Node
{
    private static Control pauseMenu;
    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;
        pauseMenu = (Control)GetParent().FindChild("PauseMenu", true, false);
        if (pauseMenu == null)
        {
            var pauseMenuScene = GD.Load<PackedScene>("res://scenes/HUD/pause_menu.tscn");
            pauseMenu = pauseMenuScene.Instantiate<Control>();
            AddChild(pauseMenu);
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("pause"))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        pauseMenu.Visible = !pauseMenu.Visible;
        GetTree().Paused = !GetTree().Paused;
    }
}
