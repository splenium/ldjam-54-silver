using Godot;
using LudumDare54Silver.scenes.PlayerController.Power;

public partial class Ghost : Node3D, Power
{
    [Export]
    public float Speed = 5.0f;
    [Export]
    public float JumpVelocity = 4.5f;
    [Export]
    public float GravityScale = 0.5f;

    [Export]
    public Area3D Detector;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float Gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public void MoveCharacter(CharacterBody3D character, double delta)
    {
        Vector3 velocity = character.Velocity;

        // Add the gravity.
        if (!character.IsOnFloor())
            velocity.Y -= Gravity * GravityScale * (float)delta;

        // Handle Jump.
        if (Input.IsActionJustPressed("ui_accept") && character.IsOnFloor())
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
    }

    public void Init(CharacterBody3D c)
    {
        c.SetCollisionMaskValue(2, false);
        c.SetCollisionLayerValue(2, true);
    }

    public void Exit(CharacterBody3D c)
    {

        c.SetCollisionMaskValue(2, true);
        //c.SetCollisionLayerValue(2, true);
    }

    /**
     * Ghost can only change to another power if there are no overlapping bodies.
     * Object is consider overlapping if it is in the same collision layer (layer 3) settings are on detector props.
     *     */
    public bool CanChange(CharacterBody3D c)
    {
        return !Detector.HasOverlappingBodies();
    }
}
