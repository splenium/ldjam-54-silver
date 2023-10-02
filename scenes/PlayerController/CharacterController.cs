using Godot;
using LudumDare54Silver.scenes.PlayerController.Power;
using System;
using System.Collections.Generic;


public partial class CharacterController : CharacterBody3D
{
	[Export]
	public Human PowerHuman;
	public string PowerHumanAction = "power_human";
	[Export]
	public bool IsPowerHumanUnlock
	{
		get => isPowerUnlock[PowerEnum.Human];
		set => isPowerUnlock[PowerEnum.Human] = value;
	}

	[Export]
	public Fish PowerFish;
	public string PowerFishAction = "power_fish";
	[Export]
	public bool IsPowerFishUnlock
	{
		get => isPowerUnlock[PowerEnum.Fish];
		set => isPowerUnlock[PowerEnum.Fish] = value;
	}

	[Export]
	public Fly PowerFly;
	public string PowerFlyAction = "power_fly";
	[Export]
	public bool IsPowerFlyUnlock
	{
		get => isPowerUnlock[PowerEnum.Fly];
		set => isPowerUnlock[PowerEnum.Fly] = value;
	}


	[Export]
	public Ghost PowerGhost;
	public string PowerGhostAction = "power_ghost";
	[Export]
	public bool IsPowerGhostUnlock
	{
		get => isPowerUnlock[PowerEnum.Ghost];
		set => isPowerUnlock[PowerEnum.Ghost] = value;
	}

	//[Export]
	//public CpuParticles2D UnlockPowerParticle;

	private Dictionary<PowerEnum, bool> isPowerUnlock = new Dictionary<PowerEnum, bool>()
	{
		{ PowerEnum.Human, true },
		{ PowerEnum.Fish, false },
		{ PowerEnum.Fly, false },
		{ PowerEnum.Ghost, false }
	};

	private Dictionary<PowerEnum, Power> powerByEnum;

	[Export]
	public Area3D DamageDetector;

	[Export]
	public Timer DamageTakenTimer;

	[Export]
	public Timer BlinkTimer;

	[Export]
	public Area3D VortexDetector;

	[Export]
	public AudioStream DeathSoundEffect;


	private Power currentPower;
	private bool invulernability = false;
	private int health = 100;

	private bool hasTeleport = false;
	private float forcedZ;
	private bool visibleState = true;

	//private Power selectedPower;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();


	
	private Sprite2D rakoon_Power;
	private Texture2D rakoon_base_texture;
	private Texture2D rakoon_wing_texture;
	private Texture2D rakoon_ghost_texture;
	private Texture2D rakoon_triton_texture;

	public override void _Ready()
	{
		base._Ready();
		GameManager.MyPlayer = this;
		forcedZ = this.GlobalPosition.Z;
		currentPower = PowerHuman;
		currentPower.Init(this);
		if (PowerHuman == null)
		{
			GD.PrintErr("CharacterController: PowerHuman is null");
		}
		if (PowerFish == null)
		{
			GD.PrintErr("CharacterController: PowerFish is null");
		}
		if (PowerFly == null)
		{
			GD.PrintErr("CharacterController: PowerFly is null");
		}
		if (PowerGhost == null)
		{
			GD.PrintErr("CharacterController: PowerGhost is null");
		}
		if (DamageDetector == null)
		{
			GD.PrintErr("CharacterController: DamageDetector is null");
		}
		if (DamageTakenTimer == null)
		{
			GD.PrintErr("CharacterController: DamageTakenTimer is null");
		}

		powerByEnum = new Dictionary<PowerEnum, Power>()
		{
			{ PowerEnum.Human, PowerHuman },
			{ PowerEnum.Fish, PowerFish },
			{ PowerEnum.Fly, PowerFly },
			{ PowerEnum.Ghost, PowerGhost }
		};

		rakoon_Power = GetParent().GetNode<Sprite2D>("Rakoon_Power");
		rakoon_base_texture = GD.Load<Texture2D>("res://scenes/dev/UI/Player/Faces_with_power/racoon_base.png");
		rakoon_wing_texture = GD.Load<Texture2D>("res://scenes/dev/UI/Player/Faces_with_power/rakoon_wing.png");
		rakoon_ghost_texture = GD.Load<Texture2D>("res://scenes/dev/UI/Player/Faces_with_power/rakoon_ghost.png");
		rakoon_triton_texture = GD.Load<Texture2D>("res://scenes/dev/UI/Player/Faces_with_power/rakoon_triton.png");
	}

	private void SelectPower(Power newPower)
	{
		currentPower.Exit(this);
		currentPower = newPower;
		GD.Print(newPower.PowerLabel);
		if (newPower.PowerLabel == "RaKoon")
		{
			rakoon_Power.Texture = rakoon_base_texture;
		}
		if (newPower.PowerLabel == "Flykoon")
		{
			rakoon_Power.Texture = rakoon_wing_texture;
		}
		if (newPower.PowerLabel == "Gostkoon")
		{
			rakoon_Power.Texture = rakoon_ghost_texture;
		}
		if (newPower.PowerLabel == "Fishkoon")
		{
			rakoon_Power.Texture = rakoon_triton_texture;
		}
		currentPower.Init(this);
	}

	public override void _Input(InputEvent @event)
	{
		if (currentPower.CanChange(this))
		{
			if (@event.IsActionReleased(PowerHumanAction) && isPowerUnlock[PowerEnum.Human])
			{
				SelectPower(PowerHuman);

				GD.Print("Human");
			}
			else if (@event.IsActionReleased(PowerFlyAction) && isPowerUnlock[PowerEnum.Fly])
			{
				SelectPower(PowerFly);

				GD.Print("Fly");
			}
			else if (@event.IsActionReleased(PowerFishAction) && isPowerUnlock[PowerEnum.Fish])
			{
				SelectPower(PowerFish);

				GD.Print("Fish");
			}
			else if (@event.IsActionReleased(PowerGhostAction) && isPowerUnlock[PowerEnum.Ghost])
			{
				SelectPower(PowerGhost);

				GD.Print("Ghost");
			}
		}
	}

	public override void _Process(double delta)
	{
		this.GlobalPosition = new Vector3(this.GlobalPosition.X, this.GlobalPosition.Y, forcedZ);
		if (!invulernability && DamageDetector.HasOverlappingAreas())
		{
			int damageTaken = GameManager.DefaultAmountOfDamage;
			foreach (Area3D area in DamageDetector.GetOverlappingAreas())
			{
				if (area is CustomDamage)
				{
					damageTaken = (area as CustomDamage).AmountOfDamage;
				}
			}
			GD.Print("Vie: ", health, "Moins: ", damageTaken);
			TakeDamage(damageTaken);
		}

		if (!hasTeleport && VortexDetector.HasOverlappingAreas())
		{
			hasTeleport = true;
			GD.Print("Vortex");
			LoadNewLevel("res://scenes/VortexTravel.tscn");
		}

		if (invulernability)
		{
			this.currentPower.Visible = this.visibleState;
		}
		else
		{
			this.currentPower.Visible = true;
		}
	}

	public async void Reset()
	{
		// Wait 10 frames to avoid the player multi death
		for (int i = 0; i < 10; i++)
		{
			await ToSignal(GetTree(), "process_frame");
		}
		health = GameManager.MaxHealth;
		DamageTakenTimer.Stop();
		invulernability = false;
	}

	public void TakeDamage(int amount)
	{
		if (invulernability)
		{
			return;
		}
		health -= amount;
		invulernability = true;

		GD.Print("Check ", health);
		if (health <= 0)
		{
			GD.Print("Mort");
			GameManager.OnPlayerRespawn(this);
			GameManager.PlaySoundEffect(DeathSoundEffect);
		}
		else
		{
			DamageTakenTimer.Start();
			BlinkTimer.Start();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (currentPower != null)
		{
			currentPower.MoveCharacter(this, delta);
		}
	}

	public void _OnDamageTakenTimerTimeout()
	{
		invulernability = false;
		GD.Print("Can take damage");
	}

	public void UnlockPower(PowerEnum power)
	{
		isPowerUnlock[power] = true;
		SelectPower(powerByEnum[power]);
		//UnlockPowerParticle.Emitting = true;
	}

	private void LoadNewLevel(string path)
    {
        Level level = GetParent() as Level;
        //GameManager.NextScene = GameManager.AllScenePath[GameManager.SceneToLoad++]; // On fait pas ca ca marche pas en gdscirpt !!!!
        GameManager.SceneToLoad = level.NextLevelNumber;
        GameManager.NextScene = GameManager.AllScenePath[GameManager.SceneToLoad];
        GD.Print("currentSceneName: ", GetTree().CurrentScene.Name, " ", GameManager.SceneToLoad, " ", GameManager.NextScene);
        GetTree().ChangeSceneToFile(path);
    }

	public void _OnBlinkTimerTimeout()
	{
		GD.Print("Change VISIBLE");
		this.visibleState = !this.visibleState;

		if (invulernability)
		{
			BlinkTimer.Start();
		}
		else
		{
			this.visibleState = true;
		}
	}
}
