using System.Collections.Generic;
using Data.Items;

namespace Implementation.Data
{
    [DbContextConfiguration("InventoryData")]
    public interface IInventoryData : IUniqueIndex
    {
        /// <summary>
        /// Gets or sets list of all slots in inventory,
        /// </summary>
        List<ISlotData> Slots { get; set; }

        /// <summary>
        /// Defines what item will be used.
        /// </summary>
        /// <param name="itemToAdd"></param>
        /// <returns>Value indicating if adding item to inventory was successful or not.</returns>
        bool AddItemToInventory(Item itemToAdd);
    }
}
