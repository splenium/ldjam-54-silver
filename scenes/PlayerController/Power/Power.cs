using Godot;

namespace LudumDare54Silver.scenes.PlayerController.Power
{
    public interface Power
    {
        void Init(CharacterBody3D character);
        void MoveCharacter(CharacterBody3D character, double delta);
        void Exit(CharacterBody3D character);
    }
}
