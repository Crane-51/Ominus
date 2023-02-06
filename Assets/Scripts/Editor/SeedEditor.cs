using UnityEngine;
using UnityEditor;
using Implementation.Data;
using General.Enums;

[CustomEditor(typeof(SeedScript))]
public class SeedEditor : Editor
{
	SeedScript objToSerialize;
	SerializedObject tgt;
	bool filesAndGuiMatch = true;
	protected static bool showKeybindsSettings = false;
	protected static bool showPlayerSettings = false;
	protected static bool showEnemySettings = true;
	protected static bool showMonkSettings = false;
	protected static bool showSkelArcherSettings = false;
	protected static bool showSkelSwordSettings = false;
	protected static bool showShamanSettings = false;
	protected static bool showWeaponSettings = true;
	protected static bool showNormalPunchSettings = false;
	protected static bool showSoldierPunchSettings = false;
	protected static bool showKnifeSettings = false;
	protected static bool showSwordSettings = false;
	protected static bool showBowSettings = false;
	protected static bool showHookSettings = false;

	void OnEnable()
	{
		objToSerialize = (SeedScript)base.target;
		tgt = new SerializedObject(objToSerialize);
		filesAndGuiMatch = CheckFiles();
	}

	public override void OnInspectorGUI()
	{
		tgt.Update();

		EditorGUILayout.Space();
		EditorGUILayout.Space();

		SerializedProperty keybindsProp = tgt.FindProperty("keybindsData");
		SerializedProperty playerStatsProp = tgt.FindProperty("playerStats");
		SerializedProperty playerMovProp = tgt.FindProperty("playerMovement");
		SerializedProperty monkStatsProp = tgt.FindProperty("monkStats");
		SerializedProperty monkDataProp = tgt.FindProperty("monkData");
		SerializedProperty monkMovProp = tgt.FindProperty("monkMovement");
		SerializedProperty skelArcherStatsProp = tgt.FindProperty("skelArcherStats");
		SerializedProperty skelArcherDataProp = tgt.FindProperty("skelArcherData");
		SerializedProperty skelArcherMovProp = tgt.FindProperty("skelArcherMovement");
		SerializedProperty skelSwordStatsProp = tgt.FindProperty("skelSwordStats");
		SerializedProperty skelSwordDataProp = tgt.FindProperty("skelSwordData");
		SerializedProperty skelSwordMovProp = tgt.FindProperty("skelSwordMovement");
		SerializedProperty shamanStatsProp = tgt.FindProperty("shamanStats");
		SerializedProperty shamanDataProp = tgt.FindProperty("shamanData");
		SerializedProperty normalPunchProp = tgt.FindProperty("normalPunch");
		SerializedProperty soldierPunchProp = tgt.FindProperty("soldierPunch");
		SerializedProperty knifeProp = tgt.FindProperty("knife");
		SerializedProperty swordProp = tgt.FindProperty("sword");
		SerializedProperty bowProp = tgt.FindProperty("bow");
		SerializedProperty hookProp = tgt.FindProperty("hook");

		if (!filesAndGuiMatch)
		{
			EditorGUILayout.HelpBox("The inspector values don't match values in files. Please save the new values or revert to saved.", MessageType.Warning);
		}

		GUILayout.BeginHorizontal();
		GUILayout.Space(Screen.width / 2 - 100);
		if (GUILayout.Button("Revert To Saved", GUILayout.Width(200)))
		{
			#region Extract values from file
			IPlayerKeybindsData keybindsData = SaveAndLoadData<IPlayerKeybindsData>.LoadSpecificData("Keybinds");
			if (keybindsData != null)
			{
				ExtractValuesFromFile(keybindsData, ref keybindsProp);
			}

			IStats playerStats = SaveAndLoadData<IStats>.LoadSpecificData("Player");
			if (playerStats != null)
			{
				ExtractValuesFromFile(playerStats, ref playerStatsProp);
			}

			IMovementData playerMov = SaveAndLoadData<IMovementData>.LoadSpecificData("Player");
			if (playerMov != null)
			{
				ExtractValuesFromFile(playerMov, ref playerMovProp);
			}

			IStats monkStats = SaveAndLoadData<IStats>.LoadSpecificData("Monk");
			if (monkStats != null)
			{
				ExtractValuesFromFile(monkStats, ref monkStatsProp);
			}

			IEnemyData monkData = SaveAndLoadData<IEnemyData>.LoadSpecificData("Monk");
			if (monkData != null)
			{
				ExtractValuesFromFile(monkData, ref monkDataProp);
			}

			IMovementData monkMov = SaveAndLoadData<IMovementData>.LoadSpecificData("Monk");
			if (monkMov != null)
			{
				ExtractValuesFromFile(monkMov, ref monkMovProp);
			}

			IStats skelArcherStats = SaveAndLoadData<IStats>.LoadSpecificData("SkeletonArcher");
			if (skelArcherStats != null)
			{
				ExtractValuesFromFile(skelArcherStats, ref skelArcherStatsProp);
			}

			IEnemyData skelArcherData = SaveAndLoadData<IEnemyData>.LoadSpecificData("SkeletonArcher");
			if (skelArcherData != null)
			{
				ExtractValuesFromFile(skelArcherData, ref skelArcherDataProp);
			}

			IMovementData skelArcherMov = SaveAndLoadData<IMovementData>.LoadSpecificData("SkeletonArcher");
			if (skelArcherMov != null)
			{
				ExtractValuesFromFile(skelArcherMov, ref skelArcherMovProp);
			}

			IStats skelSwordStats = SaveAndLoadData<IStats>.LoadSpecificData("SkeletonSword");
			if (skelSwordStats != null)
			{
				ExtractValuesFromFile(skelSwordStats, ref skelSwordStatsProp);
			}

			IEnemyData skelSwordData = SaveAndLoadData<IEnemyData>.LoadSpecificData("SkeletonSword");
			if (skelSwordData != null)
			{
				ExtractValuesFromFile(skelSwordData, ref skelSwordDataProp);
			}

			IMovementData skelSwordMov = SaveAndLoadData<IMovementData>.LoadSpecificData("SkeletonSword");
			if (skelSwordMov != null)
			{
				ExtractValuesFromFile(skelSwordMov, ref skelSwordMovProp);
			}

			IStats shamanStats = SaveAndLoadData<IStats>.LoadSpecificData("Shaman");
			if (shamanStats != null)
			{
				ExtractValuesFromFile(shamanStats, ref shamanStatsProp);
			}

			IEnemyData shamanData = SaveAndLoadData<IEnemyData>.LoadSpecificData("Shaman");
			if (shamanData != null)
			{
				ExtractValuesFromFile(shamanData, ref shamanDataProp);
			}


			IWeaponData normalPunch = SaveAndLoadData<IWeaponData>.LoadSpecificData("NormalPunch");
			if (normalPunch != null)
			{
				ExtractValuesFromFile(normalPunch, ref normalPunchProp);
			}

			IWeaponData soldierPunch = SaveAndLoadData<IWeaponData>.LoadSpecificData("SoldierPunch");
			if (soldierPunch != null)
			{
				ExtractValuesFromFile(soldierPunch, ref soldierPunchProp);
			}

			IWeaponData knife = SaveAndLoadData<IWeaponData>.LoadSpecificData("Knife");
			if (knife != null)
			{
				ExtractValuesFromFile(knife, ref knifeProp);
			}

			IWeaponData sword = SaveAndLoadData<IWeaponData>.LoadSpecificData("Sword");
			if (knife != null)
			{
				ExtractValuesFromFile(sword, ref swordProp);
			}

			IWeaponData bow = SaveAndLoadData<IWeaponData>.LoadSpecificData("Bow");
			if (knife != null)
			{
				ExtractValuesFromFile(bow, ref bowProp);
			}

			IWeaponData hook = SaveAndLoadData<IWeaponData>.LoadSpecificData("GrapplingHook");
			if (hook != null)
			{
				ExtractValuesFromFile(hook, ref hookProp);
			}
			#endregion

			filesAndGuiMatch = CheckFiles();
		}
		GUILayout.EndHorizontal();
		EditorGUILayout.Space();
		EditorGUILayout.Space();


		#region Display settings
		showKeybindsSettings = EditorGUILayout.Foldout(showKeybindsSettings, "Keybinds Settings");
		if (showKeybindsSettings)
		{
			EditorGUILayout.PropertyField(keybindsProp);
		}

		EditorGUILayout.Space();
		EditorGUILayout.Space();

		showPlayerSettings = EditorGUILayout.Foldout(showPlayerSettings, "Player Settings");
		if (showPlayerSettings)
		{
			EditorGUILayout.PropertyField(playerStatsProp);
			EditorGUILayout.PropertyField(playerMovProp);
		}

		EditorGUILayout.Space();
		EditorGUILayout.Space();

		showEnemySettings = EditorGUILayout.Foldout(showEnemySettings, "Enemy Settings");
		if (showEnemySettings)
		{
			EditorGUI.indentLevel++;
			showMonkSettings = EditorGUILayout.Foldout(showMonkSettings, "Monk Settings");
			if (showMonkSettings)
			{
				EditorGUILayout.PropertyField(monkStatsProp);
				EditorGUILayout.PropertyField(monkDataProp);
				EditorGUILayout.PropertyField(monkMovProp);
			}

			showSkelArcherSettings = EditorGUILayout.Foldout(showSkelArcherSettings, "Skeleton Archer Settings");
			if (showSkelArcherSettings)
			{
				EditorGUILayout.PropertyField(skelArcherStatsProp);
				EditorGUILayout.PropertyField(skelArcherDataProp);
				EditorGUILayout.PropertyField(skelArcherMovProp);
			}

			showSkelSwordSettings = EditorGUILayout.Foldout(showSkelSwordSettings, "Skeleton Swordman Settings");
			if (showSkelSwordSettings)
			{
				EditorGUILayout.PropertyField(skelSwordStatsProp);
				EditorGUILayout.PropertyField(skelSwordDataProp);
				EditorGUILayout.PropertyField(skelSwordMovProp);
			}

			showShamanSettings = EditorGUILayout.Foldout(showShamanSettings, "Shaman Settings");
			if (showShamanSettings)
			{
				EditorGUILayout.PropertyField(shamanStatsProp);
				//EditorGUILayout.PropertyField(shamanDataProp);
			}
			EditorGUI.indentLevel--;
		}

		EditorGUILayout.Space();
		EditorGUILayout.Space();

		showWeaponSettings = EditorGUILayout.Foldout(showWeaponSettings, "Weapon Settings");
		if (showWeaponSettings)
		{
			EditorGUI.indentLevel++;
			showNormalPunchSettings = EditorGUILayout.Foldout(showNormalPunchSettings, "Normal Punch Settings");
			if (showNormalPunchSettings)
			{
				EditorGUILayout.PropertyField(normalPunchProp);
			}

			showSoldierPunchSettings = EditorGUILayout.Foldout(showSoldierPunchSettings, "Soldier's Punch Settings");
			if (showSoldierPunchSettings)
			{
				EditorGUILayout.PropertyField(soldierPunchProp);
			}

			showKnifeSettings = EditorGUILayout.Foldout(showKnifeSettings, "Knife Settings");
			if (showKnifeSettings)
			{
				EditorGUILayout.PropertyField(knifeProp);
			}

			showSwordSettings = EditorGUILayout.Foldout(showSwordSettings, "Sword Settings");
			if (showSwordSettings)
			{
				EditorGUILayout.PropertyField(swordProp);
			}

			showBowSettings = EditorGUILayout.Foldout(showBowSettings, "Bow Settings");
			if (showBowSettings)
			{
				EditorGUILayout.PropertyField(bowProp);
			}

			showHookSettings = EditorGUILayout.Foldout(showHookSettings, "Grappling Hook Settings");
			if (showHookSettings)
			{
				EditorGUILayout.PropertyField(hookProp);
			}
			EditorGUI.indentLevel--;
		}

		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		#endregion

		GUILayout.BeginHorizontal();
		GUILayout.Space(Screen.width / 2 - 100);
		if (GUILayout.Button("Save Values", GUILayout.Width(200)))
		{
			#region Save values
			IPlayerKeybindsData keybindsData = new PlayerKeybindsData();
			PrepareValuesForSaving(keybindsProp, "Keybinds", ref keybindsData);
			SaveAndLoadData<IPlayerKeybindsData>.SaveData(keybindsData);

			var breakable = new Stats()
			{
				Id = "Breakable",
				MaxHealth = 1,
				CurrentHealth = 1
			};
			SaveAndLoadData<IStats>.SaveData(breakable);

			IStats playerStats = new Stats();
			PrepareValuesForSaving(playerStatsProp, "Player", ref playerStats);
			SaveAndLoadData<IStats>.SaveData(playerStats);

			IMovementData playerMov = new MovementData();
			PrepareValuesForSaving(playerMovProp, "Player", ref playerMov);
			SaveAndLoadData<IMovementData>.SaveData(playerMov);

			IStats monkStats = new Stats();
			PrepareValuesForSaving(monkStatsProp, "Monk", ref monkStats);
			SaveAndLoadData<IStats>.SaveData(monkStats);

			IEnemyData monkData = new EnemyData();
			PrepareValuesForSaving(monkDataProp, "Monk", ref monkData);
			SaveAndLoadData<IEnemyData>.SaveData(monkData);

			IMovementData monkMov = new MovementData();
			PrepareValuesForSaving(monkMovProp, "Monk", ref monkMov);
			SaveAndLoadData<IMovementData>.SaveData(monkMov);

			IStats skelArcherStats = new Stats();
			PrepareValuesForSaving(skelArcherStatsProp, "SkeletonArcher", ref skelArcherStats);
			SaveAndLoadData<IStats>.SaveData(skelArcherStats);

			IEnemyData skelArcherData = new EnemyData();
			PrepareValuesForSaving(skelArcherDataProp, "SkeletonArcher", ref skelArcherData);
			SaveAndLoadData<IEnemyData>.SaveData(skelArcherData);

			IMovementData skelArcherMov = new MovementData();
			PrepareValuesForSaving(skelArcherMovProp, "SkeletonArcher", ref skelArcherMov);
			SaveAndLoadData<IMovementData>.SaveData(skelArcherMov);

			IStats skelSwordStats = new Stats();
			PrepareValuesForSaving(skelSwordStatsProp, "SkeletonSwordman", ref skelSwordStats);
			SaveAndLoadData<IStats>.SaveData(skelSwordStats);

			IEnemyData skelSwordData = new EnemyData();
			PrepareValuesForSaving(skelSwordDataProp, "SkeletonSwordman", ref skelSwordData);
			SaveAndLoadData<IEnemyData>.SaveData(skelSwordData);

			IMovementData skelSwordMov = new MovementData();
			PrepareValuesForSaving(skelSwordMovProp, "SkeletonSwordman", ref skelSwordMov);
			SaveAndLoadData<IMovementData>.SaveData(skelSwordMov);

			IStats shamanStats = new Stats();
			PrepareValuesForSaving(shamanStatsProp, "Shaman", ref shamanStats);
			SaveAndLoadData<IStats>.SaveData(shamanStats);

			IEnemyData shamanData = new EnemyData();
			PrepareValuesForSaving(shamanDataProp, "Shaman", ref shamanData);
			SaveAndLoadData<IEnemyData>.SaveData(shamanData);


			IWeaponData normalPunch = new WeaponData();
			PrepareValuesForSaving(normalPunchProp, "NormalPunch", ref normalPunch);
			SaveAndLoadData<IWeaponData>.SaveData(normalPunch);

			IWeaponData soldierPunch = new WeaponData();
			PrepareValuesForSaving(soldierPunchProp, "SoldierPunch", ref soldierPunch);
			SaveAndLoadData<IWeaponData>.SaveData(soldierPunch);

			IWeaponData knife = new WeaponData();
			PrepareValuesForSaving(knifeProp, "Knife", ref knife);
			SaveAndLoadData<IWeaponData>.SaveData(knife);

			IWeaponData sword = new WeaponData();
			PrepareValuesForSaving(swordProp, "Sword", ref sword);
			SaveAndLoadData<IWeaponData>.SaveData(sword);

			IWeaponData bow = new WeaponData();
			PrepareValuesForSaving(bowProp, "Bow", ref bow);
			SaveAndLoadData<IWeaponData>.SaveData(bow);

			IWeaponData hook = new WeaponData();
			PrepareValuesForSaving(hookProp, "GrapplingHook", ref hook);
			SaveAndLoadData<IWeaponData>.SaveData(hook);
			#endregion

			PrefabUtility.ApplyPrefabInstance(objToSerialize.gameObject, InteractionMode.UserAction);
			filesAndGuiMatch = CheckFiles();
		}
		GUILayout.EndHorizontal();


		//Apply the changes to our list
		tgt.ApplyModifiedProperties();
	}

	public static void DrawUILine(Color color, int thickness = 2, int padding = 30)
	{
		Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
		r.height = thickness;
		r.y += padding / 2;
		r.x -= 2;
		r.width += 6;
		EditorGUI.DrawRect(r, color);
	}

	private void ExtractValuesFromFile(IPlayerKeybindsData data, ref SerializedProperty prop)
	{
		prop.FindPropertyRelative("keyboardUp").intValue = (int)data.KeyboardUp;
		prop.FindPropertyRelative("keyboardDown").intValue = (int)data.KeyboardDown;
		prop.FindPropertyRelative("keyboardLeft").intValue = (int)data.KeyboardLeft;
		prop.FindPropertyRelative("keyboardRight").intValue = (int)data.KeyboardRight;
		prop.FindPropertyRelative("keyboardUse").intValue = (int)data.KeyboardUse;
		prop.FindPropertyRelative("keyboardJump").intValue = (int)data.KeyboardJump;
		prop.FindPropertyRelative("keyboardPunchKey").intValue = (int)data.KeyboardPunchKey;
		prop.FindPropertyRelative("keyboardStealthKey").intValue = (int)data.KeyboardStealthKey;
		prop.FindPropertyRelative("keyboardCrouchKey").intValue = (int)data.KeyboardCrouchKey;
		prop.FindPropertyRelative("keyboardInventory").intValue = (int)data.KeyboardInventory;
		prop.FindPropertyRelative("keyboardDodge").intValue = (int)data.KeyboardDodge;
	}

	private void PrepareValuesForSaving(SerializedProperty prop, string id, ref IPlayerKeybindsData stats)
	{
		stats = new PlayerKeybindsData()
		{
			Id = id,
			KeyboardUp = (KeyCode)prop.FindPropertyRelative("keyboardUp").intValue,
			KeyboardDown = (KeyCode)prop.FindPropertyRelative("keyboardDown").intValue,
			KeyboardLeft = (KeyCode)prop.FindPropertyRelative("keyboardLeft").intValue,
			KeyboardRight = (KeyCode)prop.FindPropertyRelative("keyboardRight").intValue,
			KeyboardUse = (KeyCode)prop.FindPropertyRelative("keyboardUse").intValue,
			KeyboardJump = (KeyCode)prop.FindPropertyRelative("keyboardJump").intValue,
			KeyboardPunchKey = (KeyCode)prop.FindPropertyRelative("keyboardPunchKey").intValue,
			KeyboardStealthKey = (KeyCode)prop.FindPropertyRelative("keyboardStealthKey").intValue,
			KeyboardCrouchKey = (KeyCode)prop.FindPropertyRelative("keyboardCrouchKey").intValue,
			KeyboardInventory = (KeyCode)prop.FindPropertyRelative("keyboardInventory").intValue,
			KeyboardDodge = (KeyCode)prop.FindPropertyRelative("keyboardDodge").intValue
		};
	}

	private void ExtractValuesFromFile(IStats stat, ref SerializedProperty prop)
	{
		prop.FindPropertyRelative("maxHealth").intValue = stat.MaxHealth;
		prop.FindPropertyRelative("currentHealth").intValue = stat.CurrentHealth;
		prop.FindPropertyRelative("strength").intValue = stat.Strength;
		prop.FindPropertyRelative("dexterity").intValue = stat.Dexterity;
		prop.FindPropertyRelative("weapon").enumValueIndex = (int)stat.Weapon;
	}

	private void PrepareValuesForSaving(SerializedProperty prop, string id, ref IStats stats)
	{
		stats = new Stats()
		{
			Id = id,
			MaxHealth = prop.FindPropertyRelative("maxHealth").intValue,
			//MaxMana = playerStatsProp.FindPropertyRelative("maxMana").intValue,
			//MaxImagination = playerStatsProp.FindPropertyRelative("maxImagination").intValue,
			//CurrentMana = playerStatsProp.FindPropertyRelative("currentMana").intValue,
			//CurrentImagination = playerStatsProp.FindPropertyRelative("currentImagination").intValue,
			CurrentHealth = prop.FindPropertyRelative("currentHealth").intValue,
			Strength = prop.FindPropertyRelative("strength").intValue,
			Dexterity = prop.FindPropertyRelative("dexterity").intValue,
			Weapon = (WeaponId)prop.FindPropertyRelative("weapon").enumValueIndex
		};
	}

	private void ExtractValuesFromFile(IEnemyData stat, ref SerializedProperty prop)
	{
		prop.FindPropertyRelative("attackCooldown").floatValue = stat.AttackCooldown;
		prop.FindPropertyRelative("canAttack").boolValue = stat.CanAttack;
		prop.FindPropertyRelative("minRangeOfAttack").floatValue = stat.MinRangeOfAttack;
		prop.FindPropertyRelative("maxRangeOfAttack").floatValue = stat.MaxRangeOfAttack;
		prop.FindPropertyRelative("rangeOfVision").floatValue = stat.RangeOfVision;
		prop.FindPropertyRelative("angleOfVision").floatValue = stat.AngleOfVision;
	}

	private void PrepareValuesForSaving(SerializedProperty prop, string id, ref IEnemyData data)
	{
		data = new EnemyData()
		{
			Id = id,
			AttackCooldown = prop.FindPropertyRelative("attackCooldown").floatValue,
			CanAttack = prop.FindPropertyRelative("canAttack").boolValue,
			MinRangeOfAttack = prop.FindPropertyRelative("minRangeOfAttack").floatValue,
			MaxRangeOfAttack = prop.FindPropertyRelative("maxRangeOfAttack").floatValue,
			RangeOfVision = prop.FindPropertyRelative("rangeOfVision").floatValue,
			AngleOfVision = prop.FindPropertyRelative("angleOfVision").floatValue
		};
	}

	private void ExtractValuesFromFile(IMovementData stat, ref SerializedProperty prop)
	{
		prop.FindPropertyRelative("movementSpeed").floatValue = stat.MovementSpeed;
		prop.FindPropertyRelative("jumpHeightMultiplicator").floatValue = stat.JumpHeightMultiplicator;
		prop.FindPropertyRelative("gravity").floatValue = stat.Gravity;
		prop.FindPropertyRelative("gravityEqualizator").floatValue = stat.GravityEqualizator;
	}

	private void PrepareValuesForSaving(SerializedProperty prop, string id, ref IMovementData data)
	{
		data = new MovementData()
		{
			Id = id,
			MovementSpeed = prop.FindPropertyRelative("movementSpeed").floatValue,
			JumpHeightMultiplicator = prop.FindPropertyRelative("jumpHeightMultiplicator").floatValue,
			Gravity = prop.FindPropertyRelative("gravity").floatValue,
			GravityEqualizator = prop.FindPropertyRelative("gravityEqualizator").floatValue
		};
	}

	private void ExtractValuesFromFile(IWeaponData stat, ref SerializedProperty prop)
	{
		prop.FindPropertyRelative("dieToRoll").intValue = (int)stat.DieToRoll;
		prop.FindPropertyRelative("type").enumValueIndex = (int)stat.Type;
	}

	private void PrepareValuesForSaving(SerializedProperty prop, string id, ref IWeaponData data)
	{
		data = new WeaponData()
		{
			Id = id,
			DieToRoll = (Die)prop.FindPropertyRelative("dieToRoll").intValue,
			Type = (WeaponType)prop.FindPropertyRelative("type").enumValueIndex
		};

	}

	private bool CheckFiles()
	{
		SerializedProperty keybindsProp = tgt.FindProperty("keybindsData");
		SerializedProperty playerStatsProp = tgt.FindProperty("playerStats");
		SerializedProperty playerMovProp = tgt.FindProperty("playerMovement");
		SerializedProperty monkStatsProp = tgt.FindProperty("monkStats");
		SerializedProperty monkDataProp = tgt.FindProperty("monkData");
		SerializedProperty monkMovProp = tgt.FindProperty("monkMovement");
		SerializedProperty skelArcherStatsProp = tgt.FindProperty("skelArcherStats");
		SerializedProperty skelArcherDataProp = tgt.FindProperty("skelArcherData");
		SerializedProperty skelArcherMovProp = tgt.FindProperty("skelArcherMovement");
		SerializedProperty skelSwordStatsProp = tgt.FindProperty("skelSwordStats");
		SerializedProperty skelSwordDataProp = tgt.FindProperty("skelSwordData");
		SerializedProperty skelSwordMovProp = tgt.FindProperty("skelSwordMovement");
		SerializedProperty normalPunchProp = tgt.FindProperty("normalPunch");
		SerializedProperty soldierPunchProp = tgt.FindProperty("soldierPunch");
		SerializedProperty knifeProp = tgt.FindProperty("knife");
		SerializedProperty swordProp = tgt.FindProperty("sword");
		SerializedProperty bowProp = tgt.FindProperty("bow");
		SerializedProperty hookProp = tgt.FindProperty("hook");

		IPlayerKeybindsData keybindsData = SaveAndLoadData<IPlayerKeybindsData>.LoadSpecificData("Keybinds");
		IStats playerStats = SaveAndLoadData<IStats>.LoadSpecificData("Player");
		IMovementData playerMov = SaveAndLoadData<IMovementData>.LoadSpecificData("Player");
		IStats monkStats = SaveAndLoadData<IStats>.LoadSpecificData("Monk");
		IEnemyData monkData = SaveAndLoadData<IEnemyData>.LoadSpecificData("Monk");
		IMovementData monkMov = SaveAndLoadData<IMovementData>.LoadSpecificData("Monk");
		IStats skelArcherStats = SaveAndLoadData<IStats>.LoadSpecificData("SkeletonArcher");
		IEnemyData skelArcherData = SaveAndLoadData<IEnemyData>.LoadSpecificData("SkeletonArcher");
		IMovementData skelArcherMov = SaveAndLoadData<IMovementData>.LoadSpecificData("SkeletonArcher");
		IStats skelSwordStats = SaveAndLoadData<IStats>.LoadSpecificData("SkeletonSwordman");
		IEnemyData skelSwordData = SaveAndLoadData<IEnemyData>.LoadSpecificData("SkeletonSwordman");
		IMovementData skelSwordMov = SaveAndLoadData<IMovementData>.LoadSpecificData("SkeletonSwordman");
		IWeaponData normalPunch = SaveAndLoadData<IWeaponData>.LoadSpecificData("NormalPunch");
		IWeaponData soldierPunch = SaveAndLoadData<IWeaponData>.LoadSpecificData("SoldierPunch");
		IWeaponData knife = SaveAndLoadData<IWeaponData>.LoadSpecificData("Knife");
		IWeaponData sword = SaveAndLoadData<IWeaponData>.LoadSpecificData("Sword");
		IWeaponData bow = SaveAndLoadData<IWeaponData>.LoadSpecificData("Bow");
		IWeaponData hook = SaveAndLoadData<IWeaponData>.LoadSpecificData("GrapplingHook");

		return CheckFile(keybindsData, keybindsProp) && CheckFile(playerStats, playerStatsProp) && CheckFile(playerMov, playerMovProp) && CheckFile(monkStats, monkStatsProp)
			&& CheckFile(monkData, monkDataProp) && CheckFile(monkMov, monkMovProp) && CheckFile(skelArcherStats, skelArcherStatsProp) && CheckFile(skelArcherData, skelArcherDataProp)
			&& CheckFile(skelArcherMov, skelArcherMovProp) && CheckFile(skelSwordStats, skelSwordStatsProp) && CheckFile(skelSwordData, skelSwordDataProp)
			&& CheckFile(skelSwordMov, skelSwordMovProp) && CheckFile(normalPunch, normalPunchProp) && CheckFile(soldierPunch, soldierPunchProp) && CheckFile(knife, knifeProp)
			&& CheckFile(sword, swordProp) && CheckFile(bow, bowProp) && CheckFile(hook, hookProp);

	}

	private bool CheckFile(IPlayerKeybindsData data, SerializedProperty prop)
	{
		if (data != null)
		{
			if ((int)data.KeyboardLeft != prop.FindPropertyRelative("keyboardLeft").intValue || (int)data.KeyboardRight != prop.FindPropertyRelative("keyboardRight").intValue
				|| (int)data.KeyboardUp != prop.FindPropertyRelative("keyboardUp").intValue || (int)data.KeyboardDown != prop.FindPropertyRelative("keyboardDown").intValue
				|| (int)data.KeyboardJump != prop.FindPropertyRelative("keyboardJump").intValue || (int)data.KeyboardPunchKey != prop.FindPropertyRelative("keyboardPunchKey").intValue
				|| (int)data.KeyboardUse != prop.FindPropertyRelative("keyboardUse").intValue || (int)data.KeyboardStealthKey != prop.FindPropertyRelative("keyboardStealthKey").intValue
				|| (int)data.KeyboardCrouchKey != prop.FindPropertyRelative("keyboardCrouchKey").intValue || (int)data.KeyboardInventory != prop.FindPropertyRelative("keyboardInventory").intValue
				|| (int)data.KeyboardDodge != prop.FindPropertyRelative("keyboardDodge").intValue)
			{
				return false;
			}
		}
		else
		{
			return false;
		}

		return true;
	}

	private bool CheckFile(IStats data, SerializedProperty prop)
	{
		if (data != null)
		{
			if (data.CurrentHealth != prop.FindPropertyRelative("currentHealth").intValue || data.Dexterity != prop.FindPropertyRelative("dexterity").intValue
				|| data.MaxHealth != prop.FindPropertyRelative("maxHealth").intValue || data.Strength != prop.FindPropertyRelative("strength").intValue
				|| (int)data.Weapon != prop.FindPropertyRelative("weapon").enumValueIndex)
			{
				return false;
			}
		}
		else
		{
			return false;
		}

		return true;
	}

	private bool CheckFile(IMovementData data, SerializedProperty prop)
	{
		if (data != null)
		{
			if (data.Gravity != prop.FindPropertyRelative("gravity").floatValue || data.GravityEqualizator != prop.FindPropertyRelative("gravityEqualizator").floatValue
				|| data.JumpHeightMultiplicator != prop.FindPropertyRelative("jumpHeightMultiplicator").floatValue || data.MovementSpeed != prop.FindPropertyRelative("movementSpeed").floatValue)
			{
				return false;
			}
		}
		else
		{
			return false;
		}

		return true;
	}

	private bool CheckFile(IEnemyData data, SerializedProperty prop)
	{
		if (data != null)
		{
			if (data.AttackCooldown != prop.FindPropertyRelative("attackCooldown").floatValue || data.CanAttack != prop.FindPropertyRelative("canAttack").boolValue
				|| data.MinRangeOfAttack != prop.FindPropertyRelative("minRangeOfAttack").floatValue || data.RangeOfVision != prop.FindPropertyRelative("rangeOfVision").floatValue
				|| data.AngleOfVision != prop.FindPropertyRelative("angleOfVision").floatValue)
			{
				return false;
			}
		}
		else
		{
			return false;
		}

		return true;
	}

	private bool CheckFile(IWeaponData data, SerializedProperty prop)
	{
		if (data != null)
		{
			if ((int)data.DieToRoll != prop.FindPropertyRelative("dieToRoll").intValue || (int)data.Type != prop.FindPropertyRelative("type").enumValueIndex)
			{
				return false;
			}
		}
		else
		{
			return false;
		}

		return true;
	}

}
