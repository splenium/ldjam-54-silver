using Godot;

public partial class CustomDamage : Area3D
{
    [Export]
    public int AmountOfDamage = GameManager.MaxHealth;
}
