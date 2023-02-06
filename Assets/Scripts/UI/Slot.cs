using System.Collections;
using System.Linq;
using Data.Items;
using DiContainerLibrary.DiContainer;
using General.Enums;
using General.State;
using Implementation.Data;
using UnityEngine;
using UnityEngine.UI;

public class Slot : HighPriorityState
{
	[InjectDiContainter]
	private IGameInformation gameInformation { get; set; }

	public Item currentItem { get; private set; }

	private Image imageOfSlot { get; set; }

	//private InventoryItemDescription descriptionPanel { get; set; }

	private Text stackSize { get; set; }

	public InventorySlotPosition position;
	private ISlotData selectedItem { get; set; }
	[SerializeField] private Color normalAlpha = new Color(1f, 1f, 1f, 0.5f);
	[SerializeField] private Color highligthedAlpha = new Color(1f, 1f, 1f, 0.85f);
	private Color emptyColor = new Color(1f, 1f, 1f, 0f);
	private Image background;

	protected override void Initialization_State()
	{
		base.Initialization_State();
		imageOfSlot = GetComponentsInChildren<Image>().FirstOrDefault(x => x.gameObject != this.gameObject);
		//descriptionPanel = transform.parent.parent.GetComponentInChildren<InventoryItemDescription>();
		stackSize = GetComponentInChildren<Text>();
		background = GetComponent<Image>();
	}

	public bool DisplayItemInSlot(int focusIndex, bool selected)
	{
		if (gameInformation == null)
		{
			//StartCoroutine(WaitOneFrame(focusIndex, selected));
			return false;
		}

		if (gameInformation.InventoryData.Slots.Count <= 0)
		{
			RemoveItem();
			return true;
		}

		//     focusIndex = focusIndex % gameInformation.InventoryData.Slots.Count;
		//     var slotPositionIndex = focusIndex + (int)position;

		//     if (slotPositionIndex < 0 || slotPositionIndex >= gameInformation.InventoryData.Slots.Count)
		//slotPositionIndex = Math.Abs(slotPositionIndex % gameInformation.InventoryData.Slots.Count);

		//selectedItem = gameInformation.InventoryData.Slots.FirstOrDefault(x => gameInformation.InventoryData.Slots.IndexOf(x) == slotPositionIndex);
		selectedItem = gameInformation.InventoryData.Slots.FirstOrDefault(x => gameInformation.InventoryData.Slots.IndexOf(x) == focusIndex);

		if (selectedItem != null)
		{
			//Debug.Log(selectedItem.ItemsResource);
			currentItem = Resources.Load<Item>(selectedItem.ItemsResource);
			imageOfSlot.sprite = currentItem.normalSprite;
			imageOfSlot.color = selected ? highligthedAlpha : normalAlpha;
			background.color = selected ? highligthedAlpha : normalAlpha;
			imageOfSlot.preserveAspect = true;
			if (selectedItem.CurrentCapacity > 1)
			{
				stackSize.text = selectedItem.CurrentCapacity.ToString();
			}
			else
			{
				stackSize.text = string.Empty;
			}
		}
		else
		{
			RemoveItem();
		}

		return true;
	}

	/// <summary>
	/// Use item from inventory. Always used by player.
	/// </summary>
	public void Use()
	{
		if (currentItem == null)
		{
			RemoveItem();
			return;
		}

		Item item;

		if (currentItem is PuzzleItem)
		{
			item = FindObjectsOfType<PuzzleItem>().ToList().FirstOrDefault(x => x.itemId == ((SlotPuzzleData)selectedItem).PuzzleItemId);
		}
		else if (currentItem is Equipment)
		{
			item = FindObjectsOfType<Equipment>().ToList().FirstOrDefault(x => x.name == selectedItem.ItemsResource);
		}
		else
		{
			item = currentItem;
		}
		item = item.UseItem(gameInformation.Player.transform);

		if (item != null && !(item is Equipment))
		{
			selectedItem.CurrentCapacity--;

			if (selectedItem.CurrentCapacity <= 0)
			{
				RemoveItem();
			} 
		}
	}

	private void RemoveItem()
	{
		if (imageOfSlot != null)
		{
			this.imageOfSlot.sprite = null;
			imageOfSlot.color = emptyColor; 
		}
		if (background != null)
		{
			background.color = normalAlpha; 
		}
		if (stackSize != null)
		{
			if (stackSize != null) stackSize.text = string.Empty; 
		}
		currentItem = null;
		if (gameInformation != null)
		{
			gameInformation.InventoryData.Slots.Remove(selectedItem); 
		}
	}

	private IEnumerator WaitOneFrame(int focusIndex, bool selected)
	{
		yield return null;
		DisplayItemInSlot(focusIndex, selected);
	}
}
