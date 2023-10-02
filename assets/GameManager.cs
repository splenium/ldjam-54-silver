using Godot;
using System.Collections.Generic;
using System.Linq;
public partial class GameManager : Node
{
    private static Control pauseMenu;
    private static List<Checkpoint> checkpoints;
    private static Checkpoint lastCheckpoint = null;

    public static AudioStreamPlayer AudioMusic = null;
    public static AudioStreamPlayer[] MyAudioEffect = null;

    private static GameManager instance;
    public static CharacterController MyPlayer = null;

    private static int simultaneousAudioEffect = 10;

    public static string NextScene = "";

    public static int MaxHealth = 100;
    public static int DefaultAmountOfDamage = 50;

    public override void _Ready()
    {
        instance = this;
        InitAudio();

        ProcessMode = ProcessModeEnum.Always;
        pauseMenu = (Control)GetParent().FindChild("PauseMenu", true, false);
        if (pauseMenu == null)
        {
            var pauseMenuScene = GD.Load<PackedScene>("res://scenes/HUD/pause_menu.tscn");
            pauseMenu = pauseMenuScene.Instantiate<Control>();
            AddChild(pauseMenu);
        }
    }

    public static void InitializeCheckpoint(Checkpoint defaultCheckpoint)
    {
        checkpoints = new List<Checkpoint>();
        checkpoints.AddRange(instance.GetTree().GetNodesInGroup("checkpoint").Select(x => (Checkpoint)x));
        OnCheckpointEnter(defaultCheckpoint);
        foreach (var checkpoint in checkpoints)
        {
            checkpoint.BodyEntered += (Node3D player) => OnCheckpointEnter(checkpoint);
        }
    }

    public void InitAudio()
    {
        AudioMusic = new AudioStreamPlayer();
        AudioMusic.Name = "AudioMusicMain";
        GD.Print("AudioMusic: " + AudioMusic + AudioMusic.VolumeDb);
        AddChild(AudioMusic);

        MyAudioEffect = new AudioStreamPlayer[simultaneousAudioEffect];
        for (int i = 0; i < simultaneousAudioEffect; i++)
        {
            MyAudioEffect[i] = new AudioStreamPlayer();
            MyAudioEffect[i].Name = "MyAudioEffect" + i;
            AddChild(GameManager.MyAudioEffect[i]);
        }
    }

    public static void PlayMusic(AudioStream audioStream, float volume = 0, float time = 0)
    {
        AudioMusic.Stop();
        AudioMusic.Stream = audioStream;
        AudioMusic.VolumeDb = volume;
        AudioMusic.Play(time);
    }

    public static bool PlaySoundEffect(AudioStream audioStream, float volume = 0, float time = 0)
    {
        foreach (AudioStreamPlayer audioSrc in MyAudioEffect)
        {
            if (!audioSrc.Playing)
            {
                GD.Print("Play once ", audioStream.ResourceName);
                audioSrc.VolumeDb = volume;
                audioSrc.Stream = audioStream;
                audioSrc.Play(time);
                return true;
            }
        }
        return false;
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
        playerController.GlobalPosition = lastCheckpoint.SpawnPoint.GlobalPosition;
        playerController.Reset();
    }
}
