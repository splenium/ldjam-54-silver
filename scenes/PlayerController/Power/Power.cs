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

        private bool wasInWater = false;

        public override void _Ready()
        {
            Visible = false;
        }

        public virtual void Init(CharacterBody3D character)
        {
            Visible = true;
        }
        public virtual void MoveCharacter(CharacterBody3D character, double delta)
        {

        }
        public virtual void Exit(CharacterBody3D character)
        {
            Visible = false;
        }
        public virtual bool CanChange(CharacterBody3D character)
        {
            return true;
        }
        protected virtual bool InWater()
        {
            return WaterDetector.HasOverlappingAreas();
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
                velocity.Y -= gravityForce * 0.9f;
            }
            return velocity;
        }
    }
}
