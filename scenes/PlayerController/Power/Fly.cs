using Godot;
using LudumDare54Silver.scenes.PlayerController.Power;

public partial class Fly : Node3D, Power
{
    [Export]
    public float Speed = 20.0f;
    [Export]
    public float JumpVelocity = 6f;
    
    [Export]
    public AnimationPlayer animationLeftWing;
    [Export]
    public AnimationPlayer animationRightWing;

    public override void _Ready()
    {
        base._Ready();
        Visible = false;
        animationLeftWing.Play("left_wing_down");
        animationRightWing.Play("right_wing_down");
    }


    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle() * 2;

    public void MoveCharacter(CharacterBody3D character, double delta)
    {
        Vector3 velocity = character.Velocity;

        // Add the gravity.
        if (!character.IsOnFloor())
            velocity.Y -= gravity * (float)delta;
    
        bool flapUpLeft = Input.IsActionJustPressed("ui_left");
        bool flapDownLeft = Input.IsActionJustReleased("ui_left");
        bool flapUpRight = Input.IsActionJustPressed("ui_right");
        bool flapDownRight = Input.IsActionJustReleased("ui_right");

        if (flapUpLeft)
        {
            animationLeftWing.Play("left_wing_up");
        }
        if (flapDownLeft)
        {
            animationLeftWing.Play("left_wing_down");
        }
        if (flapUpRight)
        {
            animationRightWing.Play("right_wing_up");
        }
        if (flapDownRight)
        {
            animationRightWing.Play("right_wing_down");
        }

        if (flapDownLeft || flapDownRight)
        {
            velocity.Y = flapDownLeft && flapDownRight ? JumpVelocity * 2 : JumpVelocity;
        }
        if ((flapDownLeft && !flapDownRight) || (!flapDownLeft && flapDownRight))
        {
            var direction = flapDownLeft ? 1 : -1;
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
        Visible = true;
    }

    public void Exit(CharacterBody3D c)
    {
        Visible = false;
    }

    public bool CanChange(CharacterBody3D c)
    {
        return true;
    }
}
