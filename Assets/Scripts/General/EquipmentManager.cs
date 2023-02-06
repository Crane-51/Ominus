using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using General.Enums;
using System.Collections.Generic;
using Data.Items;
using System.Linq;
using UnityEngine;
using CustomCamera;

public class EquipmentManager : HighPriorityState
{
	public Equipment EquippedItem { get; set; }
	private Equipment defaultItem;
	public CrosshairMovement Crosshair { get; private set; }
	public List<IWeaponData> Weapons { get; private set; }
	[InjectDiContainter]
	protected IGameInformation gameInformation { get; set; }
	private AimingCamera aimCam;

	protected override void Initialization_State()
	{
		base.Initialization_State();
		Weapons = new List<IWeaponData>();
		defaultItem = GetComponentsInChildren<Equipment>().ToList().FirstOrDefault(x => x.name == "Fists");
		if (EquippedItem == null)
		{
			//equip fists
			if (defaultItem != null)
			{
				defaultItem.Equip();
			}
		}

		//foreach (var child in GetComponentsInChildren<Transform>())
		//{
		//	if (child.name == "Crosshair")
		//	{
		//		Crosshair = child.gameObject;
		//		Debug.Log("Found crosshair");
		//		break;
		//	}
		//}
		Crosshair = GetComponentInChildren<CrosshairMovement>();
		aimCam = gameInformation.Camera.gameObject.GetComponent<AimingCamera>();
	}

	public void Equip(Equipment item)
	{
		Debug.Log("Equipping: " + item.name);
		EquippedItem = item;
		var weapon = Weapons.FirstOrDefault(x => x.Id == item.Weapon.ToString());
		if (weapon == null)
		{
			weapon = SaveAndLoadData<IWeaponData>.LoadSpecificData(item.Weapon.ToString());
			Weapons.Add(weapon);
		}
		if (weapon.Type == WeaponType.Ranged)
		{
			EquippedRanged();
		}
	}

	public void Unequip(Equipment item)
	{
		Debug.Log("Unequipping: " + item.name);
		EquippedItem = null;
		var weapon = Weapons.FirstOrDefault(x => x.Id == item.Weapon.ToString());
		if (weapon == null)
		{
			weapon = SaveAndLoadData<IWeaponData>.LoadSpecificData(item.Weapon.ToString());
			Weapons.Add(weapon);
		}
		if (weapon.Type == WeaponType.Ranged)
		{
			UnequippedRanged();
		}
	}

	public void EquipDefault()
	{
		if (defaultItem != null)
		{
			defaultItem.Equip();
		}
	}

	public void EquippedRanged()
	{
		if (Crosshair != null)
		{
			Crosshair./*GetComponent<CrosshairMovement>().*/EnableCrosshair(true);
		}

		if (aimCam != null)
		{
			aimCam.Activate(Crosshair);
		}
	}

	public void UnequippedRanged()
	{
		if (Crosshair != null)
		{
			Crosshair./*GetComponent<CrosshairMovement>().*/EnableCrosshair(false);
		}

		if (aimCam != null)
		{
			aimCam.Deactivate();
		}
	}
}
