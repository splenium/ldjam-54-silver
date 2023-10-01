using Godot;
using System;

public partial class StartingMenu : Control
{
    [Export]
    private PackedScene StartingScene;

    public void _on_start_button_pressed()
    {
        LoadNewLevel(StartingScene.ResourcePath);
    }

    private void LoadNewLevel(string path)
    {
        var level = GD.Load<PackedScene>(path);
        var newLevelInstance = level.Instantiate();
        GetTree().Root.AddChild(newLevelInstance);
        QueueFree();
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
