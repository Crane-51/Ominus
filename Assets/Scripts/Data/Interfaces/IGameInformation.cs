using General.State;
using UnityEngine;

namespace Implementation.Data
{
    public interface IGameInformation
    {
        /// <summary>
        /// Gets or sets camera object.
        /// </summary>
        Camera Camera { get; set; }

        /// <summary>
        /// Gets or sets player object.
        /// </summary>
        GameObject Player { get; set; }

        /// <summary>
        /// Gets or sets player state controller.
        /// </summary>
        StateController PlayerStateController { get; set; }

        /// <summary>
        /// Gets or sets inventory data.
        /// </summary>
        IInventoryData InventoryData { get; set; }

        /// <summary>
        /// Gets or sets value indicating whether all movement should be stopped.
        /// </summary>
        bool StopMovement { get; set; }

		///<summary>
		/// Gets or sets value indicating if game is paused
		///</summary>
		bool IsPaused { get; set; }

		///<summary>
		/// Gets or sets value indicating if dialog is active
		///</summary>
		bool DialogActive { get; set; }

		/// <summary>
		/// Gets or sets value indicating if player can interact with Use btn
		/// </summary>
		bool WaitingForInteraction { get; set; }
    }
}
