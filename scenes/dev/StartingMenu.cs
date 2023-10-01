using Godot;
using System;

public partial class StartingMenu : Control
{
    [Export]
    private PackedScene StartingScene;

    public void _on_start_button_pressed()
    {
        GetTree().ChangeSceneToFile(StartingScene.ResourcePath);
    }
}
