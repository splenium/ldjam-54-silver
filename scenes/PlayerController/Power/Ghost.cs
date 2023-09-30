using Godot;
using LudumDare54Silver.scenes.PlayerController.Power;

public partial class Ghost : Power
{
    [Export]
    public float Speed = 4.0f;
    [Export]
    public float JumpVelocity = 4.5f;
    [Export]
    public float GravityScale = 0.5f;

    [Export]
    public Area3D Detector;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float Gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public override void _Ready()
    {
        base._Ready();
        if (Detector == null)
        {
            GD.PrintErr("Ghost: Detector is null");
        }
    }

    public override void MoveCharacter(CharacterBody3D character, double delta)
    {
        Vector3 velocity = character.Velocity;
        float gravityForce = Gravity * (float)delta;

        // Add the gravity.
        if (!character.IsOnFloor())
            velocity.Y -= gravityForce;

        velocity = WaterBehavior(gravityForce, velocity, delta);

        // Handle Jump.
        if (Input.IsActionJustPressed("ui_up") && (character.IsOnFloor() || InWater()))
            velocity.Y = JumpVelocity;

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(character.Velocity.X, 0, Speed);
        }

        character.Velocity = velocity;
        character.MoveAndSlide();

        SetRaKoonAvatarAnimation(velocity);
    }

    public override void Init(CharacterBody3D c)
    {
        base.Init(c);
        c.SetCollisionMaskValue(2, false);
    }

    public override void Exit(CharacterBody3D c)
    {
        base.Exit(c);
        c.SetCollisionMaskValue(2, true);
    }

    public override bool CanChange(CharacterBody3D c)
    {
        return IsCollidingObject();
    }

    private bool IsCollidingObject()
    {
        return !Detector.HasOverlappingBodies();
    }
}
