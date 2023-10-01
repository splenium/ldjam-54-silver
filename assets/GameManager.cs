using System.Collections.Generic;
using System.Linq;
using Godot;
public partial class GameManager : Node
{
    private static Control pauseMenu;
    private static List<Checkpoint> checkpoints = new List<Checkpoint>();
    private static Checkpoint lastCheckpoint = null;

    public override void _Ready()
    {
        checkpoints.AddRange(GetTree().GetNodesInGroup("checkpoint").Select(x => (Checkpoint)x));
        foreach (var checkpoint in checkpoints)
        {
            checkpoint.BodyEntered += (Node3D player) => OnCheckpointEnter(checkpoint);
        }
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

    public static void OnCheckpointEnter(Area3D checkpointArea3D)
    {
        var checkpoint = (Checkpoint)checkpointArea3D;
        if (!checkpoint.WasActivated)
        {
            lastCheckpoint = checkpoint;
            checkpoint.WasActivated = true;
        }
    }

    public static void OnPlayerRespawn(CharacterBody3D player)
    {
        var playerController = (CharacterController)player;
        playerController.Reset();
        playerController.GlobalPosition = lastCheckpoint.SpawnPoint.GlobalPosition;
    }
}
