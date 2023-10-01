using Godot;

namespace LudumDare54Silver.scenes.PlayerController.Power
{
    public partial class Power : Node3D
    {
        [Export]
        public Area3D WaterDetector;

        [Export]
        public float ArchimedesScale = 2f;
        [Export]
        public float ContactWaterBreak = 0.2f;
        [Export]
        public RaKoonAvatar raKoonAvatar;
        [Export]
        public Color LightColor;
        [Export]
        public PowerEnum PowerEnum;

        private bool wasInWater = false;

        public override void _Ready()
        {
            Visible = false;
            ProcessMode = ProcessModeEnum.Disabled;
            if (raKoonAvatar == null)
            {
                GD.PrintErr("Power: raKoonAvatar is null");
            }
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
        }

        public virtual void Init(CharacterBody3D character)
        {
            ProcessMode = ProcessModeEnum.Inherit;
            Visible = true;
            raKoonAvatar.LightColor = LightColor;
        }
        public virtual void MoveCharacter(CharacterBody3D character, double delta)
        {

        }
        public virtual void Exit(CharacterBody3D character)
        {
            Visible = false;
            ProcessMode = ProcessModeEnum.Disabled;
        }
        public virtual bool CanChange(CharacterBody3D character)
        {
            return true;
        }
        protected virtual bool InWater()
        {
            return WaterDetector.HasOverlappingAreas() || WaterDetector.HasOverlappingBodies();
        }

        protected virtual void SetRaKoonAvatarAnimation(Vector3 velocity)
        {
            if (velocity.X > 0)
            {
                raKoonAvatar.IsLeft = false;
            }
            else if (velocity.X < 0)
            {
                raKoonAvatar.IsLeft = true;
            }
            raKoonAvatar.IsMoving = !velocity.IsZeroApprox();
        }

        protected virtual Vector3 WaterBehavior(float gravityForce, Vector3 velocity, double delta)
        {
            if (InWater())
            {
                if (!wasInWater)
                {
                    velocity.Y = velocity.Y * (1f - ContactWaterBreak);
                }
                wasInWater = true;
                velocity.Y += gravityForce * ArchimedesScale;
            }
            else if (wasInWater)
            {
                wasInWater = false;
                velocity.Y -= gravityForce * 0.8f;
            }
            return velocity;
        }
    }
}

public enum PowerEnum
{
    Human,
    Fish,
    Fly,
    Ghost
}
