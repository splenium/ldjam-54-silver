using System.Collections.Generic;
using Godot;
using LudumDare54Silver.scenes.PlayerController.Power;

public partial class CharacterController : CharacterBody3D
{
    [Export]
    public Human PowerHuman;
    public string PowerHumanAction = "power_human";
    [Export]
    public bool IsPowerHumanUnlock {
        get => isPowerUnlock[PowerEnum.Human];
        set => isPowerUnlock[PowerEnum.Human] = value;
    }

    [Export]
    public Fish PowerFish;
    public string PowerFishAction = "power_fish";
    [Export]
    public bool IsPowerFishUnlock {
        get => isPowerUnlock[PowerEnum.Fish];
        set => isPowerUnlock[PowerEnum.Fish] = value;
    }

    [Export]
    public Fly PowerFly;
    public string PowerFlyAction = "power_fly";
    [Export]
    public bool IsPowerFlyUnlock {
        get => isPowerUnlock[PowerEnum.Fly];
        set => isPowerUnlock[PowerEnum.Fly] = value;
    }


    [Export]
    public Ghost PowerGhost;
    public string PowerGhostAction = "power_ghost";
    [Export]
    public bool IsPowerGhostUnlock {
        get => isPowerUnlock[PowerEnum.Ghost];
        set => isPowerUnlock[PowerEnum.Ghost] = value;
    }

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

    [Signal]
    public delegate void PlayerDiedEventHandler();

    private Power currentPower;
    private bool invulernability = false;
    private int health = 100;

    //private Power selectedPower;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public override void _Ready()
    {
        base._Ready();
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
                GD.Print("Human");
            }
            else if (@event.IsActionReleased(PowerFlyAction) && isPowerUnlock[PowerEnum.Fly])
            {
                SelectPower(PowerFly);
                GD.Print("Fly");
            }
            else if (@event.IsActionReleased(PowerFishAction) && isPowerUnlock[PowerEnum.Fish])
            {
                SelectPower(PowerFish);
                GD.Print("Fish");
            }
            else if (@event.IsActionReleased(PowerGhostAction) && isPowerUnlock[PowerEnum.Ghost])
            {
                SelectPower(PowerGhost);
                GD.Print("Ghost");
            }
        }
    }

    public override void _Process(double delta)
    {
        if (!invulernability && DamageDetector.HasOverlappingAreas())
        {
            GD.Print("Bim: ", health);
            TakeDamage(50);
        }
    }

    public void Reset()
    {
        health = 100;
        // invulernability = false;
    }

    public void TakeDamage(int amount)
    {
        if (invulernability)
        {
            return;
        }
        health -= amount;
        GD.Print("Check ", health);
        if (health <= 0)
        {
            GD.Print("Mort");
            DamageTakenTimer.Stop();
            EmitSignal(nameof(PlayerDiedEventHandler));
            GameManager.OnPlayerRespawn(this);
        }
        else
        {
            GD.Print("Tranquille");
            invulernability = true;
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
    }

}
