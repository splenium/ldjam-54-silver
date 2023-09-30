using Godot;
using LudumDare54Silver.scenes.PlayerController.Power;

public partial class CharacterController : CharacterBody3D
{
    [Export]
    public Human powerHuman;
    [Export]
    public string powerHumanAction = "power_human";

    [Export]
    public Fish powerFish;
    [Export]
    public string powerFishAction = "power_fish";

    [Export]
    public Fly powerFly;
    [Export]
    public string powerFlyAction = "power_fly";

    [Export]
    public Ghost powerGhost;
    [Export]
    public string powerGhostAction = "power_ghost";

    private Power currentPower;

    //private Power selectedPower;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public override void _Ready()
    {
        base._Ready();
        currentPower = powerHuman;
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

            if (@event.IsActionReleased(powerHumanAction))
            {
                SetNewPower(powerHuman);
                GD.Print("Human");
            }
            else if (@event.IsActionReleased(powerFlyAction))
            {
                SetNewPower(powerFly);
                GD.Print("Fly");
            }
            else if (@event.IsActionReleased(powerFishAction))
            {
                SetNewPower(powerFish);
                GD.Print("Fish");
            }
            else if (@event.IsActionReleased(powerGhostAction))
            {
                SetNewPower(powerGhost);
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
