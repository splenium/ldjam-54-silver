using Godot;
using LudumDare54Silver.scenes.PlayerController.Power;

public partial class Fish : Power
{
    [Export]
    public float MaxSpeed = 6f;
    [Export]
    public float Acceleration = 10f;
    [Export]
    public float GravityScaleUnderwater = 0.05f;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float Gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    private bool active = false;

    public override void MoveCharacter(CharacterBody3D character, double delta)
    {
        Vector3 velocity = character.Velocity;

        if (InWater())
        {
            float gravity = Gravity * GravityScaleUnderwater * (float)delta;
            velocity.Y -= gravity;

            // Add the gravity.

            // Get the input direction and handle the movement/deceleration.
            // As good practice, you should replace UI actions with custom gameplay actions.
            Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_down", "ui_up");
            Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, inputDir.Y, 0)).Normalized();
            if (direction.X != Vector3.Zero.X)
            {
                velocity.X += direction.X * Acceleration * (float)delta;
            }
            else
            {
                velocity.X = Mathf.MoveToward(character.Velocity.X, 0, Acceleration * (float)delta);
            }

            if (direction.Y != Vector3.Zero.Y)
            {
                velocity.Y += direction.Y * Acceleration * (float)delta + gravity;
            }

            if (Mathf.Abs(velocity.Y) > MaxSpeed)
            {
                if (velocity.Y > 0)
                {
                    velocity.Y = MaxSpeed;
                }
                else
                {
                    velocity.Y = -MaxSpeed;
                }
            }
            if (Mathf.Abs(velocity.X) > MaxSpeed)
            {
                if (velocity.X > 0)
                {
                    velocity.X = MaxSpeed;
                }
                else
                {
                    velocity.X = -MaxSpeed;
                }
            }
            GD.Print(velocity);
        }
        else if (!character.IsOnFloor())
        {
            velocity.Y -= Gravity * (float)delta;
            velocity.X = Mathf.MoveToward(character.Velocity.X, 0, MaxSpeed);
        }
        else
        {
            velocity.X = Mathf.MoveToward(character.Velocity.X, 0, MaxSpeed);
        }

        character.Velocity = velocity;
        character.MoveAndSlide();
    }
}
