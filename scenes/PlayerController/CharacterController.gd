extends CharacterBody3D

class_name CharacterController

@export var PowerHuman: Human
var PowerHumanAction: String = "power_human"
@export var IsPowerHumanUnlock: bool:
    get: return isPowerUnlock[Enums.Power.Human]
    set(value): isPowerUnlock[Enums.Power.Human] = value

@export var PowerFish: Fish
var PowerFishAction: String = "power_fish"
@export var IsPowerFishUnlock: bool:
    get: return isPowerUnlock[Enums.Power.Fish]
    set(value): isPowerUnlock[Enums.Power.Fish] = value

@export var PowerFly: Fly
var PowerFlyAction: String = "power_fly"
@export var IsPowerFlyUnlock: bool:
    get: return isPowerUnlock[Enums.Power.Fly]
    set(value): isPowerUnlock[Enums.Power.Fly] = value

@export var PowerGhost: Ghost
var PowerGhostAction: String = "power_ghost"
@export var IsPowerGhostUnlock: bool:
    get: return isPowerUnlock[Enums.Power.Ghost]
    set(value): isPowerUnlock[Enums.Power.Ghost] = value

@export var DamageDetector: Area3D
@export var DamageTakenTimer: Timer
@export var BlinkTimer: Timer
@export var VortexDetector: Area3D
@export var DeathSoundEffect: AudioStream

var isPowerUnlock = {
    Enums.Power.Human: true,
    Enums.Power.Fish: false,
    Enums.Power.Fly: false,
    Enums.Power.Ghost: false,
}

var powerByEnum: Dictionary

var currentPower: Power
var invulernability: bool = false
var health: int = 100

var hasTeleport: bool = false
var forcedZ: float
var visibleState: bool = true

var gravity: float = ProjectSettings.get_setting("physics/3d/default_gravity")

var rakoon_Power: Sprite2D
var rakoon_base_texture: Texture2D
var rakoon_wing_texture: Texture2D
var rakoon_ghost_texture: Texture2D
var rakoon_triton_texture: Texture2D

var wingsOff: Sprite2D
var ghostOff: Sprite2D
var tritonOff: Sprite2D

var wingsOn: Texture2D
var ghostOn: Texture2D
var tritonOn: Texture2D

func _ready():
    GameManager.MyPlayer = self
    forcedZ = self.global_position.z
    powerByEnum = {
        Enums.Power.Human: PowerHuman,
        Enums.Power.Fish: PowerFish,
        Enums.Power.Fly: PowerFly,
        Enums.Power.Ghost: PowerGhost,
    }
    currentPower = PowerHuman
    currentPower.Init(self)
    if PowerHuman == null:
        push_error("CharacterController: PowerHuman is null")
    if PowerFish == null:
        push_error("CharacterController: PowerFish is null")
    if PowerFly == null:
        push_error("CharacterController: PowerFly is null")
    if PowerGhost == null:
        push_error("CharacterController: PowerGhost is null")
    if DamageDetector == null:
        push_error("CharacterController: DamageDetector is null")
    if DamageTakenTimer == null:
        push_error("CharacterController: DamageTakenTimer is null")

    rakoon_Power = get_parent().get_node("Rakoon_Power")
    rakoon_base_texture = load("res://scenes/dev/UI/Player/Faces_with_power/racoon_base.png")
    rakoon_wing_texture = load("res://scenes/dev/UI/Player/Faces_with_power/rakoon_wing.png")
    rakoon_ghost_texture = load("res://scenes/dev/UI/Player/Faces_with_power/rakoon_ghost.png")
    rakoon_triton_texture = load("res://scenes/dev/UI/Player/Faces_with_power/rakoon_triton.png")

    wingsOff = get_parent().get_node("Power_icons/WingsOff")
    wingsOn = load("res://scenes/dev/UI/Player/Power_on/wings_on.png")
    if isPowerUnlock[Enums.Power.Fly]:
        wingsOff.texture = wingsOn

    ghostOff = get_parent().get_node("Power_icons/GhostOff")
    ghostOn = load("res://scenes/dev/UI/Player/Power_on/ghost_on.png")
    if isPowerUnlock[Enums.Power.Ghost]:
        ghostOff.texture = ghostOn

    tritonOff = get_parent().get_node("Power_icons/TritonOff")
    tritonOn = load("res://scenes/dev/UI/Player/Power_on/triton_on.png")
    if isPowerUnlock[Enums.Power.Fish]:
        tritonOff.texture = tritonOn

func SelectPower(newPower: Power) -> void:
    currentPower.Exit(self)
    currentPower = newPower
    print(newPower.PowerLabel)
    if newPower.PowerLabel == "RaKoon":
        rakoon_Power.texture = rakoon_base_texture
    if newPower.PowerLabel == "Flykoon":
        rakoon_Power.texture = rakoon_wing_texture
    if newPower.PowerLabel == "Gostkoon":
        rakoon_Power.texture = rakoon_ghost_texture
    if newPower.PowerLabel == "Fishkoon":
        rakoon_Power.texture = rakoon_triton_texture
    currentPower.Init(self)

func _input(event: InputEvent) -> void:
    if currentPower.CanChange(self):
        if event.is_action_released(PowerHumanAction) and isPowerUnlock[Enums.Power.Human]:
            SelectPower(PowerHuman)
            print("Human")
        elif event.is_action_released(PowerFlyAction) and isPowerUnlock[Enums.Power.Fly]:
            SelectPower(PowerFly)
            print("Fly")
        elif event.is_action_released(PowerFishAction) and isPowerUnlock[Enums.Power.Fish]:
            SelectPower(PowerFish)
            print("Fish")
        elif event.is_action_released(PowerGhostAction) and isPowerUnlock[Enums.Power.Ghost]:
            SelectPower(PowerGhost)
            print("Ghost")

func _process(_delta: float) -> void:
    self.global_position = Vector3(self.global_position.x, self.global_position.y, forcedZ)
    if not invulernability and DamageDetector.has_overlapping_areas():
        var damageTaken: int = GameManager.DefaultAmountOfDamage
        for area in DamageDetector.get_overlapping_areas():
            if area is CustomDamage:
                damageTaken = (area as CustomDamage).AmountOfDamage
        print("Vie: ", health, "Moins: ", damageTaken)
        TakeDamage(damageTaken)

    if not hasTeleport and VortexDetector.has_overlapping_areas():
        hasTeleport = true
        print("Vortex")
        LoadNewLevel("res://scenes/VortexTravel.tscn")

    if invulernability:
        self.currentPower.visible = self.visibleState
    else:
        self.currentPower.visible = true

func Reset() -> void:
    # Wait 10 frames to avoid the player multi death
    for i in range(10):
        await get_tree().process_frame
    health = GameManager.MaxHealth
    DamageTakenTimer.stop()
    invulernability = false

func TakeDamage(amount: int) -> void:
    if invulernability:
        return
    health -= amount
    invulernability = true
    print("Check ", health)
    if health <= 0:
        print("Mort")
        GameManager.OnPlayerRespawn(self)
        GameManager.PlaySoundEffect(DeathSoundEffect)
    else:
        DamageTakenTimer.start()
        BlinkTimer.start()

func _physics_process(delta: float) -> void:
    if currentPower != null:
        currentPower.MoveCharacter(self, delta)

func _OnDamageTakenTimerTimeout() -> void:
    invulernability = false
    print("Can take damage")

func UnlockPower(power: Enums.Power) -> void:
    isPowerUnlock[power] = true
    if power == Enums.Power.Ghost:
        ghostOff.texture = ghostOn
    elif power == Enums.Power.Fish:
        tritonOff.texture = tritonOn
    elif power == Enums.Power.Fly:
        wingsOff.texture = wingsOn
    SelectPower(powerByEnum[power])

func LoadNewLevel(path: String) -> void:
    var level: Level = get_parent()
    GameManager.SceneToLoad = level.NextLevelNumber
    GameManager.NextScene = GameManager.AllScenePath[GameManager.SceneToLoad]
    print("currentSceneName: ", get_tree().current_scene.name, " ", GameManager.SceneToLoad, " ", GameManager.NextScene)
    get_tree().change_scene_to_file(path)

func _OnBlinkTimerTimeout() -> void:
    print("Change VISIBLE")
    self.visibleState = !self.visibleState
    if invulernability:
        BlinkTimer.start()
    else:
        self.visibleState = true
