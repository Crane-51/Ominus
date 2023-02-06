using Implementation.Data;
using UnityEngine;

public class SeedScript : MonoBehaviour
{
	public PlayerKeybindsData keybindsData;

	public Stats playerStats;
	public MovementData playerMovement;

	public Stats monkStats;
	public EnemyData monkData;
	public MovementData monkMovement;

	public Stats skelArcherStats;
	public EnemyData skelArcherData;
	public MovementData skelArcherMovement;

	public Stats skelSwordStats;
	public EnemyData skelSwordData;
	public MovementData skelSwordMovement;

	public Stats shamanStats;
	public EnemyData shamanData;

	public WeaponData normalPunch;
	public WeaponData soldierPunch;
	public WeaponData knife;
	public WeaponData sword;
	public WeaponData bow;
	public WeaponData hook;


	//void Start()
	//{


	//	SeedCharacter();
	//	SeedMovement();
	//	SeedCamera();
	//	SeedEnemyData();
	//	SeedKeybindings();
	//	SeedPlayerStats();
	//	SeedInventory();
	//	SeedDialog();
	//}

	void Start()
	{
		if (!SaveAndLoadData<IPlayerKeybindsData>.CheckIfExists())
		{
			keybindsData.Id = "Keybinds";
			SaveAndLoadData<IPlayerKeybindsData>.SaveData(keybindsData);
		}

		if (!SaveAndLoadData<IStats>.CheckIfExists())
		{
			playerStats.Id = "Player";
			monkStats.Id = "Monk";
			skelArcherStats.Id = "SkeletonArcher";
			skelSwordStats.Id = "SkeletonSwordman";
			var breakable = new Stats()
			{
				Id = "Breakable",
				MaxHealth = 1,
				CurrentHealth = 1
			};
			SaveAndLoadData<IStats>.SaveData(playerStats);
			SaveAndLoadData<IStats>.SaveData(monkStats);
			SaveAndLoadData<IStats>.SaveData(skelArcherStats);
			SaveAndLoadData<IStats>.SaveData(skelSwordStats);
			SaveAndLoadData<IStats>.SaveData(breakable);
		}

		if (!SaveAndLoadData<IMovementData>.CheckIfExists())
		{
			playerMovement.Id = "Player";
			monkMovement.Id = "Monk";
			skelArcherMovement.Id = "SkeletonArcher";
			skelSwordMovement.Id = "SkeletonSwordman";
			SaveAndLoadData<IMovementData>.SaveData(playerMovement);
			SaveAndLoadData<IMovementData>.SaveData(monkMovement);
			SaveAndLoadData<IMovementData>.SaveData(skelArcherMovement);
			SaveAndLoadData<IMovementData>.SaveData(skelSwordMovement);
		}

		if (!SaveAndLoadData<IEnemyData>.CheckIfExists())
		{
			monkData.Id = "Monk";
			skelArcherData.Id = "SkeletonArcher";
			skelSwordData.Id = "SkeletonSwordman";
			SaveAndLoadData<IEnemyData>.SaveData(monkData);
			SaveAndLoadData<IEnemyData>.SaveData(skelArcherData);
			SaveAndLoadData<IEnemyData>.SaveData(skelSwordData);
		}

		if (!SaveAndLoadData<IWeaponData>.CheckIfExists())
		{
			normalPunch.Id = "NormalPunch";
			soldierPunch.Id = "SoldierPunch";
			knife.Id = "Knife";
			sword.Id = "Sword";
			bow.Id = "Bow";
			SaveAndLoadData<IWeaponData>.SaveData(normalPunch);
			SaveAndLoadData<IWeaponData>.SaveData(soldierPunch);
			SaveAndLoadData<IWeaponData>.SaveData(knife);
			SaveAndLoadData<IWeaponData>.SaveData(sword);
			SaveAndLoadData<IWeaponData>.SaveData(bow);
		}

		SeedInventory();

		//if (!SaveAndLoadData<IWholeDialogBoxData>.CheckIfExists())
		//{
		//	SeedDialog();
		//}
	}

	private void SeedInventory()
	{
		var inventory = new InventoryData()
		{
			Id = "Inventory",
		};

		inventory.Slots.Add(new SlotData()
		{
			CurrentCapacity = 1,
			ItemsResource = "HealthPotion"
		});

		//GameObject player = GameObject.FindGameObjectWithTag("Player");
		//Equipment[] equipment = player.GetComponentsInChildren<Equipment>();

		//foreach (var item in equipment)
		//{
		//	inventory.Slots.Add(new SlotData()
		//	{
		//		ItemsResource = item.name
		//	}); 
		//}

		SaveAndLoadData<IInventoryData>.SaveData(inventory);
		//Debug.Log(inventory.Id);
	}

	//private void SeedPlayerStats()
	//{
	//	var playerStats = new Stats()
	//	{
	//		Id = "Player",
	//		MaxHealth = 50,
	//		MaxMana = 3,
	//		MaxImagination = 10,
	//		CurrentMana = 1,
	//		CurrentImagination = 4,
	//		CurrentHealth = 50,
	//	};

	//	SaveAndLoadData<IStats>.SaveData(playerStats);

	//	foreach (var item in SaveAndLoadData<IStats>.LoadAllData())
	//	{
	//		Debug.Log("MaxHealth ->" + item.MaxHealth);
	//		Debug.Log("MaxMana ->" + item.MaxMana);
	//		Debug.Log("MaxImagination ->" + item.MaxImagination);
	//		Debug.Log("CurrentHealth ->" + item.CurrentHealth);
	//		Debug.Log("CurrentMana ->" + item.CurrentMana);
	//		Debug.Log("CurrentImagination ->" + item.CurrentImagination);
	//	}
	//}

	//private void SeedKeybindings()
	//{
	//	var binds = new PlayerKeybindsData()
	//	{
	//		Id = "Keybinds",
	//		KnockdownKey = KeyCode.U,
	//		KeyboardPunchKey = KeyCode.D,
	//		SpellAction1 = KeyCode.I,
	//		KeyboardStealthKey = KeyCode.S,
	//		KeyboardLeft = KeyCode.LeftArrow,
	//		KeyboardRight = KeyCode.RightArrow,
	//		KeyboardUp = KeyCode.UpArrow,
	//		KeyboardDown = KeyCode.DownArrow,
	//		KeyboardJump = KeyCode.Space,
	//		KeyboardUse = KeyCode.E,
	//		JoystickJump = KeyCode.Joystick1Button0,
	//		JoystickPunchKey = KeyCode.Joystick1Button1,
	//		JoystickStealthKey = KeyCode.Joystick1Button2,
	//		JoystickUse = KeyCode.Joystick1Button3
	//	};

	//	SaveAndLoadData<IPlayerKeybindsData>.SaveData(binds);

	//	foreach (var item in SaveAndLoadData<IPlayerKeybindsData>.LoadAllData())
	//	{
	//		Debug.Log("Action key 1 ->" + item.KnockdownKey);
	//		Debug.Log("Action key 2 ->" + item.KeyboardPunchKey);
	//		Debug.Log("Action key 3 ->" + item.SpellAction1);
	//		Debug.Log("Left ->" + item.KeyboardLeft);
	//		Debug.Log("Right ->" + item.KeyboardRight);
	//		Debug.Log("Jump ->" + item.KeyboardJump);
	//	}
	//}

	//private void SeedEnemyData()
	//{
	//	var enemy = new EnemyData()
	//	{
	//		Id = "Enemy",
	//		AngleOfVisionHigher = 120,
	//		AngleOfVisionLower = 0,
	//		MinRangeOfAttack = 1,
	//		RangeOfVision = 10f,
	//		AttackCooldown = 1,
	//		Damage = 1,
	//		CanAttack = true
	//	};

	//	SaveAndLoadData<IEnemyData>.SaveData(enemy);

	//	foreach (var item in SaveAndLoadData<IEnemyData>.LoadAllData())
	//	{
	//		Debug.Log("Id" + item.Id);
	//		Debug.Log("Angle of vision higher" + item.AngleOfVisionHigher);
	//		Debug.Log("Angle of vision lower" + item.AngleOfVisionLower);
	//		Debug.Log("Range of attack" + item.MinRangeOfAttack);
	//		Debug.Log("Range of vision" + item.RangeOfVision);
	//	}
	//}

	//private void SeedDialog()
	//{
	//	var singleDialog = new SingleDialogData()
	//	{
	//		CharacterIcon = "ana",
	//		CharacterName = "Ana",
	//		Text = "Who goes there?"
	//	};
	//	var singleDialog2 = new SingleDialogData()
	//	{
	//		CharacterIcon = "mirko",
	//		CharacterName = "Mirko",
	//		Text = "It's me Mario"
	//	};
	//	var singleDialog3 = new SingleDialogData()
	//	{
	//		CharacterIcon = "ana",
	//		CharacterName = "Ana",
	//		Text = "Mario who?"
	//	};
	//	var singleDialog4 = new SingleDialogData()
	//	{
	//		CharacterIcon = "jure",
	//		CharacterName = "Jure",
	//		Text = "Can you two please keep quite I'm trying to sleep here."
	//	};
	//	var singleDialog5 = new SingleDialogData()
	//	{
	//		CharacterIcon = "ana",
	//		CharacterName = "Ana",
	//		Text = "No."
	//	};
	//	var singleDialog6 = new SingleDialogData()
	//	{
	//		CharacterIcon = "mirko",
	//		CharacterName = "Mirko",
	//		Text = "Noop."
	//	};
	//	var dialogBox = new WholeDialogData();
	//	dialogBox.Dialog.Add(1, singleDialog);
	//	dialogBox.Dialog.Add(2, singleDialog2);
	//	dialogBox.Dialog.Add(3, singleDialog3);
	//	dialogBox.Dialog.Add(4, singleDialog4);
	//	dialogBox.Dialog.Add(5, singleDialog5);
	//	dialogBox.Dialog.Add(6, singleDialog6);
	//	dialogBox.Id = "TestDialog";

	//	SaveAndLoadData<IWholeDialogBoxData>.SaveData(dialogBox);
	//}

	//private void SeedCamera()
	//{
	//	var cam = new CameraData()
	//	{
	//		Id = "Camera",
	//		FollowDistance = 5.5f,
	//		MovementSpeed = 10,
	//		ZAxisOffset = -5.5f,
	//		YAxisOffset = 4.30f
	//	};

	//	SaveAndLoadData<ICameraData>.SaveData(cam);

	//	foreach (var item in SaveAndLoadData<ICameraData>.LoadAllData())
	//	{
	//		Debug.Log("Id->" + item.Id);
	//		Debug.Log("Follow distance ->" + item.FollowDistance);
	//		Debug.Log("Movement speed ->" + item.MovementSpeed);
	//		Debug.Log("Z axis offset ->" + item.ZAxisOffset);
	//	}
	//}

	//private void SeedMovement()
	//{
	//	var chr1 = new MovementData();

	//	chr1.Id = "Player";
	//	chr1.Gravity = 4.3f;
	//	chr1.GravityEqualizator = 8;
	//	chr1.IsInAir = false;
	//	chr1.HorizontalMovement = 0;
	//	chr1.JumpHeightMultiplicator = 2.8f;
	//	chr1.MovementSpeed = 5;

	//	SaveAndLoadData<IMovementData>.SaveData(chr1);

	//	chr1.Id = "Enemy";
	//	chr1.Gravity = 4.3f;
	//	chr1.GravityEqualizator = 5;
	//	chr1.IsInAir = false;
	//	chr1.HorizontalMovement = 0;
	//	chr1.JumpHeightMultiplicator = 9;
	//	chr1.MovementSpeed = 3.5f;

	//	SaveAndLoadData<IMovementData>.SaveData(chr1);

	//	chr1.Id = "Platform";
	//	chr1.Gravity = 4.3f;
	//	chr1.GravityEqualizator = 5;
	//	chr1.IsInAir = false;
	//	chr1.HorizontalMovement = 0;
	//	chr1.JumpHeightMultiplicator = 9;
	//	chr1.MovementSpeed = 4.5f;

	//	SaveAndLoadData<IMovementData>.SaveData(chr1);

	//	chr1.Id = "Spikes";
	//	chr1.Gravity = 4.3f;
	//	chr1.GravityEqualizator = 5;
	//	chr1.IsInAir = false;
	//	chr1.HorizontalMovement = 0;
	//	chr1.JumpHeightMultiplicator = 9;
	//	chr1.MovementSpeed = 5.5f;

	//	SaveAndLoadData<IMovementData>.SaveData(chr1);

	//	foreach (var item in SaveAndLoadData<IMovementData>.LoadAllData())
	//	{
	//		Debug.Log("Id->" + item.Id);
	//		Debug.Log("Gravity->" + item.Gravity);
	//		Debug.Log("GravityEqualizator->" + item.GravityEqualizator);
	//		Debug.Log("JumpHeightMultiplicator->" + item.JumpHeightMultiplicator);
	//		Debug.Log("MovementSpeed->" + item.MovementSpeed);
	//		Debug.Log("IsInAir->" + item.IsInAir);
	//	}
	//}

	//private void SeedCharacter()
	//{
	//	var char1 = new Stats();

	//	char1.Id = "Enemy";
	//	char1.Name = "Skeleton Knight";
	//	char1.MaxHealth = 10;
	//	char1.CurrentHealth = 10;

	//	SaveAndLoadData<IStats>.SaveData(char1);

	//	var char2 = new Stats();

	//	char2.Id = "Player";
	//	char2.Name = "Player";
	//	char2.MaxHealth = 50;
	//	char2.CurrentHealth = 50;

	//	SaveAndLoadData<IStats>.SaveData(char2);

	//	var char3 = new Stats();

	//	char3.Id = "Breakable";
	//	char3.Name = "Breakable";
	//	char3.MaxHealth = 1;
	//	char3.CurrentHealth = 1;

	//	SaveAndLoadData<IStats>.SaveData(char3);

	//	foreach (var item in SaveAndLoadData<IStats>.LoadAllData())
	//	{
	//		Debug.Log("Id->" + item.Id);
	//		Debug.Log("Name->" + item.Name);
	//		Debug.Log("MHP->" + item.MaxHealth);
	//		Debug.Log("HP->" + item.CurrentHealth);
	//	}
	//}
}
