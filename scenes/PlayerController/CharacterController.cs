using Godot;
using LudumDare54Silver.scenes.PlayerController.Power;

public partial class CharacterController : CharacterBody3D
{
    [Export]
    public Human PowerHuman;
    [Export]
    public string PowerHumanAction = "power_human";

    [Export]
    public Fish PowerFish;
    [Export]
    public string PowerFishAction = "power_fish";

    [Export]
    public Fly PowerFly;
    [Export]
    public string PowerFlyAction = "power_fly";

    [Export]
    public Ghost PowerGhost;
    [Export]
    public string PowerGhostAction = "power_ghost";

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
    }

    private void SetNewPower(Power newPower)
    {
        currentPower.Exit(this);
        currentPower = newPower;
        currentPower.Init(this);
    }

    public override void _Input(InputEvent @event)
    {
        if (currentPower.CanChange(this))
        {

            if (@event.IsActionReleased(PowerHumanAction))
            {
                SetNewPower(PowerHuman);
                GD.Print("Human");
            }
            else if (@event.IsActionReleased(PowerFlyAction))
            {
                SetNewPower(PowerFly);
                GD.Print("Fly");
            }
            else if (@event.IsActionReleased(PowerFishAction))
            {
                SetNewPower(PowerFish);
                GD.Print("Fish");
            }
            else if (@event.IsActionReleased(PowerGhostAction))
            {
                SetNewPower(PowerGhost);
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
}
