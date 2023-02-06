using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data.Items;
using System.Linq;

public class PuzzleItem : Item
{
	[SerializeField] public string itemId;
	private PuzzleSlotMatching puzzle;

	protected override void Initialization_State()
	{
		base.Initialization_State();
		puzzle = GetComponentInParent<PuzzleSlotMatching>(); //or rather put it in OnEnable()?
	}

	public override Item UseItem(Transform objectThatUsesItem)
	{
		//List<PuzzleItem> items = FindObjectsOfType<PuzzleItem>().ToList();
		//items.FirstOrDefault(x => x.)

		if (puzzle.activeSlot != null)
		{
			//gameObject.SetActive(true);
			transform.position = puzzle.activeSlot.transform.position;
			GetComponent<SpriteRenderer>().enabled = true;
			GetComponent<Collider2D>().enabled = true;
			//gameObject.SetActive(true);
			puzzle.activeSlot.InsertItem(itemId);
			return this;
		}
		return null;
	}

	public void ItemPickedUp()
	{
		if (puzzle.activeSlot != null)
		{
			puzzle.activeSlot.RemoveItem();
		}
	}
}
