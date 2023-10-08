extends Node

var pauseMenu: Control
var checkpoints: Array[Checkpoint]
var lastCheckpoint: Checkpoint = null

var AudioMusic: AudioStreamPlayer = null
var MyAudioEffect: Array[AudioStreamPlayer] = []

var MyPlayer: CharacterController = null

var simultaneousAudioEffect: int = 10

var SceneToLoad: int = 1

var NextScene: String

var AllScenePath: Array = ["res://scenes/Levels/graveyard.tscn", "res://scenes/Levels/AbyssScene.tscn", "res://scenes/Levels/FlyScene.tscn", "res://scenes/Levels/LaboScene.tscn"]

var MaxHealth: int = 100
var DefaultAmountOfDamage: int = 50

func _ready():
    InitAudio()
    process_mode = PROCESS_MODE_ALWAYS
    pauseMenu = get_parent().find_child("PauseMenu", true, false) as Control
    if pauseMenu == null:
        var pauseMenuScene = load("res://scenes/HUD/pause_menu.tscn") as PackedScene
        pauseMenu = pauseMenuScene.instantiate() as Control
        add_child(pauseMenu)

func InitializeCheckpoint(defaultCheckpoint: Checkpoint) -> void:
    checkpoints = []
    checkpoints.append_array(get_tree().get_nodes_in_group("checkpoint"))
    OnCheckpointEnter(defaultCheckpoint)
    for checkpoint in checkpoints:
        # we unbind the argument send be the signal (that would be the player) to bind the checkpoint instead
        checkpoint.body_entered.connect(OnCheckpointEnter.unbind(1).bind(checkpoint))

func InitAudio() -> void:
    AudioMusic = AudioStreamPlayer.new()
    AudioMusic.name = "AudioMusicMain"
    print("AudioMusic: ", AudioMusic, AudioMusic.volume_db)
    add_child(AudioMusic)
    MyAudioEffect = []
    for i in range(0, simultaneousAudioEffect):
        MyAudioEffect.append(AudioStreamPlayer.new())
        MyAudioEffect[i].name = "MyAudioEffect" + str(i)
        add_child(MyAudioEffect[i])

func PlayMusic(audioStream: AudioStream, volume: float = 0, time: float = 0) -> void:
    AudioMusic.stop()
    AudioMusic.stream = audioStream
    AudioMusic.volume_db = volume
    AudioMusic.play(time)

func PlaySoundEffect(audioStream: AudioStream, volume: float = 0, time: float = 0) -> bool:
    for audioSrc in MyAudioEffect:
        if not audioSrc.playing:
            print("Play once ", audioStream.resource_name)
            audioSrc.volume_db = volume
            audioSrc.stream = audioStream
            audioSrc.play(time)
            return true
    return false

func _input(event: InputEvent) -> void:
    if event.is_action_pressed("pause"):
        TogglePause()

func TogglePause() -> void:
    pauseMenu.visible = !pauseMenu.visible
    get_tree().paused = !get_tree().paused

func OnCheckpointEnter(checkpointArea3D: Area3D) -> void:
    var checkpoint = checkpointArea3D as Checkpoint
    if not checkpoint.WasActivated:
        lastCheckpoint = checkpoint
        checkpoint.WasActivated = true

func OnPlayerRespawn(player: CharacterBody3D) -> void:
    var playerController = player as CharacterController
    playerController.global_position = lastCheckpoint.SpawnPoint.global_position
    playerController.Reset()
