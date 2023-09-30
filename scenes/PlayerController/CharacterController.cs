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

    private Power currentPower;

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

    public void _Collision()
    {
        GD.Print("Collision");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (currentPower != null)
        {
            currentPower.MoveCharacter(this, delta);
        }
    }
}
