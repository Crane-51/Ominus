using System.Collections.Generic;
using UnityEngine;
using DiContainerLibrary.DiContainer;
using Implementation.Data;
using System.Linq;


public class Inventory : MonoBehaviour
{
	[InjectDiContainter] private IPlayerKeybindsData keyBindings { get; set; }
	[InjectDiContainter] protected IGameInformation gameInformation { get; set; }
	private int inputValue;
	private int selectedObject;
	private List<Slot> slots;
	private int middle;
	private bool allSlotsDrawn = true;
	private EquipmentManager equipManager;

	// Start is called before the first frame update
	void Start()
    {
		DiContainerInitializor.RegisterObject(this);
		keyBindings = SaveAndLoadData<IPlayerKeybindsData>.LoadSpecificData("Keybinds");
		slots = GetComponentsInChildren<Slot>().ToList();
		selectedObject = 0;
		middle = slots.Count / 2;
		RedrawSlots();
		ShowSecondarySlots(false);
		equipManager = gameInformation.Player.GetComponent<EquipmentManager>();
	}

    // Update is called once per frame
    void Update()
    {
		if (!allSlotsDrawn)
		{
			RedrawSlots();
		}

		if (Input.GetKey(keyBindings.KeyboardInventory) && !gameInformation.DialogActive && !gameInformation.IsPaused)
		{
			if (!allSlotsDrawn)
			{
				RedrawSlots();
			}
			ShowSecondarySlots(true);
			gameInformation.StopMovement = true;
			inputValue = (Input.GetKeyUp(keyBindings.KeyboardLeft) ? 1 : 0) + (Input.GetKeyDown(keyBindings.KeyboardRight) ? -1 : 0);
			if (inputValue != 0)
			{
				selectedObject += inputValue;
				selectedObject = selectedObject < 0 ? gameInformation.InventoryData.Slots.Count - 1 : selectedObject;
				selectedObject = selectedObject > gameInformation.InventoryData.Slots.Count - 1 ? 0 : selectedObject;
				RedrawSlots();
			}
		}

		if (Input.GetKeyUp(keyBindings.KeyboardInventory))
		{
			gameInformation.StopMovement = gameInformation.IsPaused || gameInformation.DialogActive;
			ShowSecondarySlots(false);
		}

		if (Input.GetKeyDown(keyBindings.KeyboardUse) && !gameInformation.WaitingForInteraction && !gameInformation.DialogActive && !gameInformation.IsPaused)
		{
			//if (selectedObject < gameInformation.InventoryData.Slots.Count)
			if (slots[middle] != null)
			{
				slots[middle].Use();
				//check if empty => select currently equipped
				if (slots[middle].currentItem == null)
				{
					for (int i = 0; i < slots.Count; i++)
					{
						if (gameInformation.InventoryData.Slots[i].ItemsResource == equipManager.EquippedItem.name)
						{
							selectedObject = i;
							break;
						}
					}
				}
				RedrawSlots(); 
			}
		}
    }

	private void ShowSecondarySlots(bool areShowed)
	{
		for (int i = 0; i < slots.Count; i++)
		{
			slots[i].gameObject.SetActive(areShowed || i == middle);
		}
	}

	public void RedrawSlots()
	{
		int objectToDisplay;
		allSlotsDrawn = true;
		for (int i = 0; i < slots.Count; i++)
		{
			//slots[i].DisplayItemInSlot((selectedIndex + i) % slots.Count, i == selectedIndex);	
			objectToDisplay = selectedObject - (middle - i);
			objectToDisplay = objectToDisplay < 0 ? objectToDisplay + slots.Count : objectToDisplay;
			objectToDisplay = objectToDisplay > slots.Count - 1 ? objectToDisplay - slots.Count : objectToDisplay;
			allSlotsDrawn = slots[i].DisplayItemInSlot(objectToDisplay, i == middle) && allSlotsDrawn;			
		}
	}
}
