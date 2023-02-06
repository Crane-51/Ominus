using System;
using System.Linq;
using Data.Items;
using DiContainerLibrary.DiContainer;
using General.Enums;
using General.State;
using Implementation.Data;
using UnityEngine;
using UnityEngine.UI;

public class OldSlot : HighPriorityState
{
    /// <summary>
    /// Gets or sets player key binds;
    /// </summary>
    [InjectDiContainter]
    private IGameInformation gameInformation { get; set; }

    private Item currentItem { get; set; }

    private Image imageOfSlot { get; set; }

    //private InventoryItemDescription descriptionPanel { get; set; }

    private Text stackSize { get; set; }

    public InventorySlotPosition position;
    private ISlotData selectedItem { get; set; }

    protected override void Initialization_State()
    {
        base.Initialization_State();
        imageOfSlot = GetComponentInChildren<Image>();
        //descriptionPanel = transform.parent.parent.GetComponentInChildren<InventoryItemDescription>();
        stackSize = GetComponentInChildren<Text>();
		DisplayItemInSlot(0);
    }

    public void DisplayItemInSlot(int focusIndex)
    {
        if (gameInformation.InventoryData.Slots.Count <= 0)
        {
            RemoveItem();
            return;
        }

        focusIndex = focusIndex % gameInformation.InventoryData.Slots.Count;
        var slotPositionIndex = focusIndex + (int)position;

        if (slotPositionIndex < 0 || slotPositionIndex >= gameInformation.InventoryData.Slots.Count)
			slotPositionIndex = Math.Abs(slotPositionIndex % gameInformation.InventoryData.Slots.Count);

        //selectedItem = gameInformation.InventoryData.Slots.FirstOrDefault(x => gameInformation.InventoryData.Slots.IndexOf(x) == slotPositionIndex);
        selectedItem = gameInformation.InventoryData.Slots.FirstOrDefault(x => gameInformation.InventoryData.Slots.IndexOf(x) == focusIndex);

        if (selectedItem != null)
        {
            currentItem = Resources.Load<Item>(selectedItem.ItemsResource);

            if (position == InventorySlotPosition.Focus)
            {
                //imageOfSlot.sprite = currentItem.highlightedSprite;
                //descriptionPanel.SetDescription(currentItem.Description);
                if (selectedItem.CurrentCapacity > 1)
                {
                    stackSize.text = selectedItem.CurrentCapacity.ToString();
                }
                else
                {
                    stackSize.text = string.Empty;
                }

            }
            else if (focusIndex != slotPositionIndex)
            {
                imageOfSlot.sprite = currentItem.normalSprite;
            }
            else
            {
                imageOfSlot.sprite = null;
            }
        }
        else
        {
            RemoveItem();
        }
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
        currentItem.UseItem(gameInformation.Player.transform);
        selectedItem.CurrentCapacity--;

        if(selectedItem.CurrentCapacity <= 0)
        {
            RemoveItem();
        }
    }

    private void RemoveItem()
    {
        this.imageOfSlot.sprite = null;
        if(stackSize != null) stackSize.text = string.Empty;
        currentItem = null;
        gameInformation.InventoryData.Slots.Remove(selectedItem);
    }
}
