using System;
using System.Collections.Generic;
using System.Linq;
using Data.Items;

namespace Implementation.Data
{
    /// <summary>
    /// Holds implementation of <see cref="IEnemyData"/>.
    /// </summary>
    [Serializable]
    public class InventoryData : IInventoryData
    {
        /// <inheritdoc />
        public string Id { get; set; }

        /// <inheritdoc />
        public List<ISlotData> Slots { get; set; }

        public InventoryData()
        {
            this.Slots = new List<ISlotData>();
        }

        public bool AddItemToInventory(Item itemToAdd)
        {
            var halfEmptySlot = Slots.FirstOrDefault(x => x.ItemsResource == itemToAdd.GetType().Name && x.CurrentCapacity < itemToAdd.MaxCapacity);

            if(halfEmptySlot != null)
            {
                halfEmptySlot.CurrentCapacity++;
            }
            else
            {
				if (itemToAdd is PuzzleItem)
				{
					Slots.Add(new SlotPuzzleData()
					{
						CurrentCapacity = 1,
						ItemsResource = itemToAdd.GetType().Name,
						PuzzleItemId = ((PuzzleItem)itemToAdd).itemId
					});
				}
				else
				{
					Slots.Add(new SlotData()
					{
						CurrentCapacity = 1,
						ItemsResource = itemToAdd.GetType().Name
					});
				}
                
            }			
            return true;
        }
    }
}
