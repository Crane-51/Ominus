using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class PuzzleSlotMatching : MonoBehaviour
{
	[HideInInspector] public PuzzleSlot activeSlot;
	private List<PuzzleSlot> puzzleSlots;
	private List<PuzzleItem> puzzleItems;
	[SerializeField] private bool reactForEachSlot = false;
	[SerializeField] private UnityEvent onPuzzleSolvedCorrectly = new UnityEvent();
	[SerializeField] private UnityEvent onPuzzleSolvedIncorrectly = new UnityEvent();
	[SerializeField] private UnityEvent onCorrectItemPlacement = new UnityEvent();
	[SerializeField] private UnityEvent onIncorrectItemPlacement = new UnityEvent();


	// Start is called before the first frame update
	void Start()
    {
		puzzleSlots = GetComponentsInChildren<PuzzleSlot>().ToList();
		puzzleItems = GetComponentsInChildren<PuzzleItem>().ToList();
    }

   public void UpdatePuzzleState(PuzzleSlot slot)
	{
		if (reactForEachSlot)
		{
			if (slot.ItemIsCorrect)
			{
				//onCorrectItemPlacement.Invoke();
				Debug.Log("Item correct");
			}
			else if (slot.ItemIsInserted && !slot.ItemIsCorrect)
			{
				//onIncorrectItemPlacement.Invoke();
				Debug.Log("Item incorrect");
			}
			else
			{
				Debug.Log("Item missing");
			}
		}

		if (puzzleSlots.All(x => x.ItemIsInserted))
		{
			if (puzzleSlots.All(x => x.ItemIsCorrect))
			{
				onPuzzleSolvedCorrectly.Invoke();
				Debug.Log("Puzzle solved correctly");
				puzzleItems.ForEach(x => x.gameObject.tag = "Puzzle");
			}
			else
			{
				//onPuzzleSolvedIncorrectly.Invoke();
				Debug.Log("Puzzle solved incorrectly");
			}
		}
	}


}
