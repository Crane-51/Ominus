using UnityEngine;
using Player.Other;
using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;

namespace Player.Movement
{
	public class PlayerMoveObject : StateForMovement
	{
		/// <summary>
		/// Gets or sets player key binds;
		/// </summary>
		[InjectDiContainter]
		protected IPlayerKeybindsData keybinds;
		private bool canMove = false;
		private bool isGrabbing = false;
		private GameObject grabbedObject;
		private float velocity;
		[InjectDiContainter] protected IGameInformation gameInformation { get; set; }
		protected EquipmentManager equipManager;
		protected Equipment currEquipped;

		protected override void Initialization_State()
		{
			base.Initialization_State();
			Priority = 3;
			keybinds = SaveAndLoadData<IPlayerKeybindsData>.LoadSpecificData("Keybinds");
			equipManager = GetComponent<EquipmentManager>();
		}

		public override void Update_State()
		{
			base.Update_State();
			gameInformation.WaitingForInteraction = gameInformation.WaitingForInteraction || canMove;
			if (canMove && Input.GetKey(keybinds.KeyboardUse))
			{
				isGrabbing = true;
				MovementData.HorizontalMovement = (Input.GetKey(keybinds.KeyboardRight) ? 1 : 0) + (Input.GetKey(keybinds.KeyboardLeft) ? -1 : 0);
			}
			else
			{
				isGrabbing = false;
			}

			if (isGrabbing && controller.ActiveStateMovement != this)
			{
				controller.SwapState(this);
			}
		}

		public override void OnEnter_State()
		{
			base.OnEnter_State();
			if (grabbedObject == null)
			{
				controller.EndState(this);
				return;
			}
			grabbedObject.GetComponent<MovableObject>().GrabObject(transform.position);

			if (equipManager.EquippedItem != null && equipManager.EquippedItem.name != "Fists")
			{
				currEquipped = equipManager.EquippedItem;
				equipManager.EquippedItem.Unequip();
			}
		}

		public override void WhileActive_State()
		{
			base.WhileActive_State();
			if (!isGrabbing || grabbedObject == null)
			{
				controller.EndState(this);
				return;
			}

			velocity = MovementData.HorizontalMovement * MovementData.MovementSpeed / 2f;
			grabbedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity, rigBody.velocity.y); 
			rigBody.velocity = new Vector2(velocity, rigBody.velocity.y);
		}

		public override void OnExit_State()
		{
			base.OnExit_State();
			if (grabbedObject != null)
			{
				grabbedObject.GetComponent<MovableObject>().ReleaseObject();
			}
			gameInformation.WaitingForInteraction = false;

			if (currEquipped != null)
			{
				currEquipped.Equip();
				currEquipped = null;
			}
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.tag == "Movable")
			{
				canMove = true;
				grabbedObject = collision.gameObject.transform.parent.gameObject;
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (collision.tag == "Movable")
			{
				canMove = false;
				gameInformation.WaitingForInteraction = false;
				//maybe if grabbing, release object?
				grabbedObject = null;
			}
		}
	}
}