using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSlot : MonoBehaviour
{
	//either needs OnActivate script or should extend it?
	[SerializeField] private string requiredItemId;
	public bool ItemIsInserted { get; private set; } = false;
	public bool ItemIsCorrect { get; private set; } = false;
	private PuzzleSlotMatching puzzle;
	private bool playerInSlotRange = false;

	private void Start()
	{
		puzzle = GetComponentInParent<PuzzleSlotMatching>();
	}

	public void InsertItem(string itemId)
	{
		ItemIsInserted = true;
		if (itemId == requiredItemId)
		{
			ItemIsCorrect = true;
		}
		else
		{
			ItemIsCorrect = false;
		}
		puzzle.UpdatePuzzleState(this);
	}

	public void RemoveItem()
	{
		ItemIsInserted = false;
		ItemIsCorrect = false;
		puzzle.UpdatePuzzleState(this);
	}
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		puzzle.activeSlot = this;
		playerInSlotRange = true;
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		puzzle.activeSlot = null;
		playerInSlotRange = false;
		
	}
}
