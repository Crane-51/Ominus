using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data.Items;
using Character.Stats;
using General.Enums;

public class Equipment : Item
{
	private Transform character;
	private SpriteRenderer sr;
	private EquipmentManager equipManager;
	public WeaponId Weapon { get { return weapon; } private set { weapon = value; } }
	[SerializeField] private WeaponId weapon;
	//public GameObject Bullet { get { return bullet; } private set { bullet = value; } }
	//[SerializeField] private GameObject bullet;
	public PoolObjectKey Bullet { get { return bullet; } private set { bullet = value; } }
	[SerializeField] private PoolObjectKey bullet;
	public float Cooldown { get { return cooldown; } private set { cooldown = value; } }
	[SerializeField] private float cooldown;
	[SerializeField] private bool rotateWithAim = true;

	protected override void Initialization_State()
	{
		base.Initialization_State();
		//Debug.Log("init equipment: " + name);
		sr = GetComponent<SpriteRenderer>();
		character = GameObject.FindGameObjectWithTag("Player").transform;
		equipManager = character.GetComponent<EquipmentManager>();
	}

	public override void Update_State()
	{
		base.Update_State();
		if (sr != null && sr.enabled && rotateWithAim)
		{
			Vector2 direction = (equipManager.Crosshair.transform.position - transform.position).normalized;
			Quaternion rotation = Quaternion.FromToRotation(new Vector2(character.localScale.x, 0f), direction);
			//if (character.localScale.x == -1)
			//{
			//	//Vector3 rot = rotation.eulerAngles;
			//	//rot = new Vector3(rot.x, rot.y, rot.z + 180);
			//	//rotation = Quaternion.Euler(rot);
			//	Vector3 rot = rotation.eulerAngles;
			//	rot = new Vector3(rot.x, 180, rot.z);
			//	rotation = Quaternion.Euler(rot);
			//}
			//else
			//{
			//	Vector3 rot = rotation.eulerAngles;
			//	rot = new Vector3(rot.x, 0, rot.z);
			//	rotation = Quaternion.Euler(rot);
			//}
			transform.rotation = rotation;
		}
	}

	public override Item UseItem(Transform objectThatUsesItem)
	{
		//character = objectThatUsesItem;
		//csm = character.GetComponent<CharacterStatsMono>();

		// check if this item is already equipped
		if (equipManager.EquippedItem == null)
		{
			Equip();
		}
		else if (equipManager.EquippedItem == this)
		{
			if (this.name != "Fists")
			{
				Unequip();
				equipManager.EquipDefault();
			}
		}
		else
		{
			// Unequip old item
			equipManager.EquippedItem.Unequip();
			// Equip new item
			Equip();
		}


		return this; // means it was successfull, otherwise return null
	}

	// maybe public if there will be a need to equip/unequip in other ways
	public void Equip()
	{		
		if (sr != null)
		{
			sr.enabled = true;

		}
		// player.Animator.SetBool("WeaponOn", true);
		// show crosshair if ranged weapon
		if (character == null)
		{
			character = GameObject.FindGameObjectWithTag("Player").transform;
			equipManager = character.GetComponent<EquipmentManager>();
		}
		if (character != null)
		{
			equipManager.Equip(this);
			//Debug.Log("Equipping: " + this.name);
			//equipManager.EquippedItem = this;
			//if (true)
			//{
			//	equipManager.EquippedRanged();
			//}
		}
	}

	public void Unequip()
	{
		if (sr != null)
		{
			sr.enabled = false; 
		}
		// player.Animator.SetBool("WeaponOn", false);
		// hide crosshair
		RopeSystem rope = GetComponent<RopeSystem>();
		if (rope != null)
		{
			rope.DetachRope();
		}
		if (character != null)
		{
			equipManager.Unequip(this);
			//Debug.Log("Unequipping: " + this.name);
			//equipManager.EquippedItem = null;
			//equipManager.UnequippedRanged();
		}		
	}

	public void HideSprite()
	{
		if (sr != null)
		{
			sr.enabled = false;
		}
	}

	public void ShowSprite()
	{
		if (sr != null && equipManager.EquippedItem == this)
		{
			sr.enabled = true;
		}
	}
}
