using Data.Items;
using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;

namespace Assets.Scripts.Characters.Player.Other
{
    public class PlayerPickUpItem : HighPriorityState
    {
        /// <summary>
        /// Gets or sets player key binds;
        /// </summary>
        [InjectDiContainter]
        protected IPlayerKeybindsData keybinds { get; set; }

        /// <summary>
        /// Gets or sets player key binds;
        /// </summary>
        [InjectDiContainter]
        protected IGameInformation gameInformation { get; set; }
		private GameObject item;
		private bool playerNearItem = false;

        protected override void Initialization_State()
        {
            base.Initialization_State();
            keybinds = SaveAndLoadData<IPlayerKeybindsData>.LoadSpecificData("Keybinds");
        }

		public override void Update_State()
		{
			base.Update_State();
			gameInformation.WaitingForInteraction = gameInformation.WaitingForInteraction || playerNearItem;
			if (/*gameInformation.WaitingForInteraction &&*/ playerNearItem && Input.GetKeyDown(keybinds.KeyboardUse))
			{
				gameInformation.WaitingForInteraction = false;
				playerNearItem = false;
				var itemComponent = item.GetComponent<Item>();
				var addedSuccesfully = gameInformation.InventoryData.AddItemToInventory(itemComponent);

				if (addedSuccesfully)
				{
					FindObjectOfType<Inventory>().RedrawSlots();

					if (itemComponent is PuzzleItem)
					{
						((PuzzleItem)itemComponent).ItemPickedUp();
						item.GetComponent<SpriteRenderer>().enabled = false;
						item.GetComponent<Collider2D>().enabled = false;
						//item.SetActive(false);
					}
					else
					{
						Destroy(item);
					}
				}
			}

		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.gameObject.tag == "Item")
			{
				//gameInformation.WaitingForInteraction = true;
				playerNearItem = true;
				item = collision.gameObject;
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (collision.gameObject.tag == "Item")
			{
				gameInformation.WaitingForInteraction = false;
				playerNearItem = false;
			}
		}

		//private void OnTriggerStay2D(Collider2D collision)
  //      {
  //          if (collision.gameObject.tag == "Item" && Input.GetKeyDown(keybinds.KeyboardUse))
  //          {
  //              var itemComponent = collision.GetComponent<Item>();

  //              var addedSuccesfully = gameInformation.InventoryData.AddItemToInventory(itemComponent);

  //              if(addedSuccesfully)
  //              {
		//			if (itemComponent is PuzzleItem)
		//			{
		//				collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
		//			}
		//			else
		//			{
		//				Destroy(collision.gameObject);
		//			}
  //              }
  //          }
  //      }
    }
}
