using General.State;
using UnityEngine;

namespace Implementation.Data
{
    public class GameInformation : IGameInformation
    {
        /// <inheritdoc />
        public Camera Camera { get; set; }

        /// <inheritdoc />
        public GameObject Player { get; set; }

        /// <inheritdoc />
        public StateController PlayerStateController { get; set; }

        /// <inheritdoc />
        public bool StopMovement { get; set; }

		/// <inheritdoc />
		public bool IsPaused { get; set; }

        /// <inheritdoc />
        public IInventoryData InventoryData { get; set; }

		/// <inheritdoc />
		public bool DialogActive { get; set; }

		/// <inheritdoc />
		public bool WaitingForInteraction { get; set; }

        public GameInformation()
        {
            Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

            Player = GameObject.Find("Player");

            InventoryData = SaveAndLoadData<IInventoryData>.LoadSpecificData("Inventory");

            if (Player != null)
            {
                PlayerStateController = Player.GetComponent<StateController>();
				GameObject player = GameObject.FindGameObjectWithTag("Player");
				Equipment[] equipment = player.GetComponentsInChildren<Equipment>();

				foreach (var item in equipment)
				{
					InventoryData.Slots.Add(new SlotData()
					{
						ItemsResource = item.name
					});
				}
			}
        }
    }
}
