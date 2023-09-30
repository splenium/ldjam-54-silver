using Godot;
using LudumDare54Silver.scenes.PlayerController.Power;

public partial class Fly : Node3D, Power
{
    [Export]
    public float Speed = 20.0f;
    [Export]
    public float JumpVelocity = 6f;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle() * 2;

    public void MoveCharacter(CharacterBody3D character, double delta)
    {
        Vector3 velocity = character.Velocity;

        // Add the gravity.
        if (!character.IsOnFloor())
            velocity.Y -= gravity * (float)delta;
    
        bool flapLeft = Input.IsActionJustReleased("ui_left");
        bool flapRight = Input.IsActionJustReleased("ui_right");
        if (flapLeft || flapRight)
        {
            velocity.Y = flapLeft && flapRight ? JumpVelocity * 2 : JumpVelocity;
        }
        if ((flapLeft && !flapRight) || (!flapLeft && flapRight))
        {
            var direction = flapLeft ? 1 : -1;
            velocity.X = direction * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(character.Velocity.X, 0, Speed / 2);
        }

        character.Velocity = velocity;
        character.MoveAndSlide();
    }

    public void Init(CharacterBody3D c)
    {

    }

    public void Exit(CharacterBody3D c)
    {

    }

    public bool CanChange(CharacterBody3D c)
    {
        return true;
    }
}
