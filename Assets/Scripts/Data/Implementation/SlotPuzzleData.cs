using System;

namespace Implementation.Data
{
	/// <summary>
	/// Holds implementation of <see cref="ISlotData"/>.
	/// </summary>
	[Serializable]
	public class SlotPuzzleData : ISlotData
	{
		/// <inheritdoc />
		public int CurrentCapacity { get; set; }

		/// <inheritdoc />
		public string ItemsResource { get; set; }

		public string PuzzleItemId { get; set; }

	}
}
