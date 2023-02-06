using UnityEngine;
using General.State;
using DiContainerLibrary.DiContainer;
using Implementation.Data;
using System.Linq;
using General.Enums;
using Character.Stats;
using System.Collections;

public class PlayerShooting : StateForMechanics
{
	private EquipmentManager equipManager;
	[InjectDiContainter]
	protected IPlayerKeybindsData keybinds;
	[SerializeField] private GameObject bulletSpawn;
	[SerializeField] private float bulletSpeed = 1f;
	private bool canAttack = true;
	//[SerializeField] private float cooldown;

	protected override void Initialization_State()
	{
		base.Initialization_State();
		Priority = 15;
		equipManager = GetComponent<EquipmentManager>();
		keybinds = SaveAndLoadData<IPlayerKeybindsData>.LoadSpecificData("Keybinds");
	}

	public override void Update_State()
	{
		base.Update_State();
		if (controller.ActiveStateMechanic != this && Input.GetKeyDown(keybinds.KeyboardPunchKey)
			&& equipManager.EquippedItem != null && equipManager.EquippedItem.name != "Fists" && canAttack)
		{
			controller.SwapState(this);
		}
	}

	public override void OnEnter_State()
	{
		base.OnEnter_State();
		RopeSystem rope = equipManager.EquippedItem.GetComponent<RopeSystem>();
		//if (rope != null)
		//{
		//	rope.Shoot(bulletSpawn.transform, bullet.transform);
		//}
		if (rope == null || !rope.RopeAttached)
		{
			Vector2 direction = (equipManager.Crosshair.transform.position - bulletSpawn.transform.position).normalized;
			Quaternion rotation = Quaternion.FromToRotation(new Vector2(transform.localScale.x, 0f), direction);
			if (transform.localScale.x == -1)
			{
				Vector3 rot = rotation.eulerAngles;
				rot = new Vector3(rot.x, rot.y, rot.z + 180);
				rotation = Quaternion.Euler(rot);
			}
			bulletSpawn.transform.rotation = rotation;
			//GameObject bullet = Instantiate(equipManager.EquippedItem.Bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
			GameObject bullet = ObjectPooler.pooler.PullObject(equipManager.EquippedItem.Bullet);

			if (bullet != null)
			{
				DmgAndDestroyOnCollision ddc = bullet.GetComponent<DmgAndDestroyOnCollision>();
				if (ddc != null)
				{
					ddc.dmgTag = "Enemy";
					IWeaponData weapon = equipManager.Weapons.FirstOrDefault(x => x.Id == equipManager.EquippedItem.Weapon.ToString());
					if (weapon != null)
					{
						ddc.dmg = DiceRoller.RollDieWithModifier(weapon.DieToRoll,
										weapon.Type == WeaponType.Melee ? GetComponent<CharacterStatsMono>().Strength : GetComponent<CharacterStatsMono>().Dexterity); 
					}
				}
				bullet.transform.position = bulletSpawn.transform.position;
				bullet.transform.rotation = bulletSpawn.transform.rotation;
				bullet.SetActive(true);
				HookBehaviour hook = bullet.GetComponent<HookBehaviour>();
				if (hook != null)
				{
					bullet.GetComponent<HookBehaviour>().SetOriginPosition(bulletSpawn.transform);
				}
				//RopeSystem rope = equipManager.EquippedItem.GetComponent<RopeSystem>();
				if (rope != null)
				{
					rope.Shoot(bulletSpawn.transform, bullet.transform);
				}
				bullet.GetComponent<Rigidbody2D>().AddForce(direction * bulletSpeed, ForceMode2D.Impulse);
				canAttack = false;
				StartCoroutine(WaitCooldown(equipManager.EquippedItem.Cooldown));
			}
		}
		else
		{
			rope.DetachRope();
		}
		controller.EndState(this);

	}

	private IEnumerator WaitCooldown(float cooldown)
	{
		yield return new WaitForSeconds(cooldown);
		canAttack = true;
	}
}
