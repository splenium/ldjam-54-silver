using Godot;
using LudumDare54Silver.scenes.PlayerController.Power;
using System.Collections.Generic;


public partial class CharacterController : CharacterBody3D
{
    [Export]
    public Human PowerHuman;
    public string PowerHumanAction = "power_human";
    [Export]
    public bool IsPowerHumanUnlock
    {
        get => isPowerUnlock[PowerEnum.Human];
        set => isPowerUnlock[PowerEnum.Human] = value;
    }

    [Export]
    public Fish PowerFish;
    public string PowerFishAction = "power_fish";
    [Export]
    public bool IsPowerFishUnlock
    {
        get => isPowerUnlock[PowerEnum.Fish];
        set => isPowerUnlock[PowerEnum.Fish] = value;
    }

    [Export]
    public Fly PowerFly;
    public string PowerFlyAction = "power_fly";
    [Export]
    public bool IsPowerFlyUnlock
    {
        get => isPowerUnlock[PowerEnum.Fly];
        set => isPowerUnlock[PowerEnum.Fly] = value;
    }


    [Export]
    public Ghost PowerGhost;
    public string PowerGhostAction = "power_ghost";
    [Export]
    public bool IsPowerGhostUnlock
    {
        get => isPowerUnlock[PowerEnum.Ghost];
        set => isPowerUnlock[PowerEnum.Ghost] = value;
    }

    //[Export]
    //public CpuParticles2D UnlockPowerParticle;

    private Dictionary<PowerEnum, bool> isPowerUnlock = new Dictionary<PowerEnum, bool>()
    {
        { PowerEnum.Human, true },
        { PowerEnum.Fish, false },
        { PowerEnum.Fly, false },
        { PowerEnum.Ghost, false }
    };

    private Dictionary<PowerEnum, Power> powerByEnum;

    [Export]
    public Area3D DamageDetector;

    [Export]
    public Timer DamageTakenTimer;

    [Export]
    public Area3D VortexDetector;

    [Export]
    public AudioStream DeathSoundEffect;


    private Power currentPower;
    private bool invulernability = false;
    private int health = 100;

    private bool hasTeleport = false;
    private float forcedZ;

    //private Power selectedPower;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
    [Export]
    private Label _rakoonStatesLabel;
    public override void _Ready()
    {
        base._Ready();
        GameManager.MyPlayer = this;
        forcedZ = this.GlobalPosition.Z;
        currentPower = PowerHuman;
        currentPower.Init(this);
        if (PowerHuman == null)
        {
            GD.PrintErr("CharacterController: PowerHuman is null");
        }
        if (PowerFish == null)
        {
            GD.PrintErr("CharacterController: PowerFish is null");
        }
        if (PowerFly == null)
        {
            GD.PrintErr("CharacterController: PowerFly is null");
        }
        if (PowerGhost == null)
        {
            GD.PrintErr("CharacterController: PowerGhost is null");
        }
        if (DamageDetector == null)
        {
            GD.PrintErr("CharacterController: DamageDetector is null");
        }
        if (DamageTakenTimer == null)
        {
            GD.PrintErr("CharacterController: DamageTakenTimer is null");
        }

        powerByEnum = new Dictionary<PowerEnum, Power>()
        {
            { PowerEnum.Human, PowerHuman },
            { PowerEnum.Fish, PowerFish },
            { PowerEnum.Fly, PowerFly },
            { PowerEnum.Ghost, PowerGhost }
        };
        //_rakoonStatesLabel = GD.Load<Label>("Panel/Etats");
    }

    private void SelectPower(Power newPower)
    {
        currentPower.Exit(this);
        currentPower = newPower;
        currentPower.Init(this);
    }

    public override void _Input(InputEvent @event)
    {
        if (currentPower.CanChange(this))
        {

            if (@event.IsActionReleased(PowerHumanAction) && isPowerUnlock[PowerEnum.Human])
            {
                SelectPower(PowerHuman);
                _rakoonStatesLabel.Text = "Rakoon";
                GD.Print("Human");
            }
            else if (@event.IsActionReleased(PowerFlyAction) && isPowerUnlock[PowerEnum.Fly])
            {
                SelectPower(PowerFly);
                _rakoonStatesLabel.Text = "Flykoon";
                GD.Print("Fly");
            }
            else if (@event.IsActionReleased(PowerFishAction) && isPowerUnlock[PowerEnum.Fish])
            {
                SelectPower(PowerFish);
                _rakoonStatesLabel.Text = "Fishkoon";
                GD.Print("Fish");
            }
            else if (@event.IsActionReleased(PowerGhostAction) && isPowerUnlock[PowerEnum.Ghost])
            {
                SelectPower(PowerGhost);
                _rakoonStatesLabel.Text = "Gostkoon";
                GD.Print("Ghost");
            }
        }
    }

    public override void _Process(double delta)
    {
        this.GlobalPosition = new Vector3(this.GlobalPosition.X, this.GlobalPosition.Y, forcedZ);
        if (!invulernability && DamageDetector.HasOverlappingAreas())
        {
            int damageTaken = GameManager.DefaultAmountOfDamage;
            foreach (Area3D area in DamageDetector.GetOverlappingAreas())
            {
                if (area is CustomDamage)
                {
                    damageTaken = (area as CustomDamage).AmountOfDamage;
                }
            }
            GD.Print("Vie: ", health, "Moins: ", damageTaken);
            TakeDamage(damageTaken);
        }

        if (!hasTeleport && VortexDetector.HasOverlappingAreas())
        {
            hasTeleport = true;
            GD.Print("Vortex");
            LoadNewLevel("res://scenes/VortexTravel.tscn");
        }
    }

    public async void Reset()
    {
        // Wait 10 frames to avoid the player multi death
        for (int i = 0; i < 10; i++)
        {
            await ToSignal(GetTree(), "process_frame");
        }
        health = GameManager.MaxHealth;
        DamageTakenTimer.Stop();
        invulernability = false;
    }

    public void TakeDamage(int amount)
    {
        if (invulernability)
        {
            return;
        }
        health -= amount;
        invulernability = true;
        GD.Print("Check ", health);
        if (health <= 0)
        {
            GD.Print("Mort");
            GameManager.OnPlayerRespawn(this);
            GameManager.PlaySoundEffect(DeathSoundEffect);
        }
        else
        {
            DamageTakenTimer.Start();
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (currentPower != null)
        {
            currentPower.MoveCharacter(this, delta);
        }
    }

    public void _OnDamageTakenTimerTimeout()
    {
        invulernability = false;
        GD.Print("Can take damage");
    }

    public void UnlockPower(PowerEnum power)
    {
        isPowerUnlock[power] = true;
        SelectPower(powerByEnum[power]);
        //UnlockPowerParticle.Emitting = true;
    }

    private void LoadNewLevel(string path)
    {
        Level level = GetParent() as Level;
        GameManager.NextScene = level.NextScene.ResourcePath;
        GD.Print("currentSceneName: ", GetTree().CurrentScene.Name);
        GetTree().ChangeSceneToFile(path);
    }
}
