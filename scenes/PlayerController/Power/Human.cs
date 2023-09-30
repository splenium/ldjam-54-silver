using Godot;
using LudumDare54Silver.scenes.PlayerController.Power;

public partial class Human : Power
{
    [Export]
    public float Speed = 5.0f;
    [Export]
    public float JumpVelocity = 4.5f;
    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float Gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

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


        raKoonAvatar.SetFace((EFaceState)(GD.Randi() % 5));

        character.Velocity = velocity;
        character.MoveAndSlide();

        SetRaKoonAvatarAnimation(velocity);
    }
}
