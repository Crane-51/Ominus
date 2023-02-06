using System;

namespace Implementation.Data
{
    /// <summary>
    /// Holds implementation of <see cref="ISlotData"/>.
    /// </summary>
    [Serializable]
    public class SlotData : ISlotData
    {
        /// <inheritdoc />
        public int CurrentCapacity { get; set; }

        /// <inheritdoc />
        public string ItemsResource { get; set; }

    }
}
