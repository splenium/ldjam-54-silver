using Godot;
using LudumDare54Silver.scenes.PlayerController.Power;

public partial class Fly : Power
{
    [Export]
    public float Speed = 2.0f;
    [Export]
    public float JumpVelocity = 6f;

    [Export]
    public AnimationPlayer AnimationLeftWing;
    [Export]
    public AnimationPlayer AnimationRightWing;

    [Export]
    public CsgMesh3D WingRight;
    [Export]
    public CsgMesh3D WingLeft;
    [Export]
    public AudioStreamPlayer BoostSound;
    [Export]
    public AudioStream[] BoostSounds;

    private WingsMouvement wingsPosition = null;

    public override void _Ready()
    {
        base._Ready();
        wingsPosition = new WingsMouvement(this);
        Visible = false;
        AnimationLeftWing.Play("left_wing_down");
        AnimationRightWing.Play("right_wing_down");
        wingsPosition.AnimationLeftWing = AnimationLeftWing;
        wingsPosition.AnimationRightWing = AnimationRightWing;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        WingRight.Material.Set("albedo_color", LightColor);
        WingLeft.Material.Set("albedo_color", LightColor);
    }


    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle() * 2;

    public override void MoveCharacter(CharacterBody3D character, double delta)
    {
        Vector3 velocity = character.Velocity;

        float gravityForce = gravity * (float)delta;

        // Add the gravity.
        if (!character.IsOnFloor())
            velocity.Y -= gravityForce;

        velocity = WaterBehavior(gravityForce, velocity, delta);

        var direction = wingsPosition.Update();


        if (direction.Y != 0)
        {
            velocity.Y = direction.Y * JumpVelocity;
        }

        if (direction.X != 0)
        {
            velocity.X += direction.X * Speed;
            velocity.X = Mathf.Clamp(velocity.X, -3 * Speed, 3 * Speed);
        }
        else
        {
            velocity.X = Mathf.MoveToward(character.Velocity.X, 0, Speed * 2 * (float)delta);
        }

        character.Velocity = velocity;
        character.MoveAndSlide();

        SetRaKoonAvatarAnimation(velocity);
    }

    public void PlayBoostSound()
    {
        /*if (BoostSound.Playing)
        {
            return;
        }
        AudioStream RngBoostSound = BoostSounds[GD.Randi() % BoostSounds.Length];
        BoostSound.Stream = RngBoostSound;
        BoostSound.Play();*/
    }

}

enum EnumWingMovement
{
    Idle,
    Down,
    Up
}