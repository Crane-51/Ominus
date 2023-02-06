using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;
using Player.Other;

namespace Player.Movement
{
	public class PlayerClimbMovement : StateForMovement
	{
		/// <summary>
		/// Gets or sets player key binds;
		/// </summary>
		[InjectDiContainter]
		protected IPlayerKeybindsData keybinds { get; set; }
		protected RaycastHit2D hit;
		protected bool canClimb = false;
		protected PlayerGravity pg;
		[InjectDiContainter]
		private IPhysicsOverlap physicsOverlap { get; set; }
		private bool climbing = false;
		private PlatformEffector2D pe;
		private Vector3 raycastCorrection = new Vector3(0f, 0f, 0f);
		protected RopeSystem rope;
		protected EquipmentManager equipManager;
		protected Equipment currEquipped;

		protected override void Initialization_State()
		{
			base.Initialization_State();
			Priority = 2;
			keybinds = SaveAndLoadData<IPlayerKeybindsData>.LoadSpecificData("Keybinds");
			pg = GetComponent<PlayerGravity>();
			rope = GetComponentInChildren<RopeSystem>();
			equipManager = GetComponent<EquipmentManager>();
		}

		public override void Update_State()
		{
			// actually it's verticalMovement in this case but we'll recycle this one
			MovementData.VerticalMovement = (Input.GetKey(keybinds.KeyboardUp) ? 1 : 0) + (Input.GetKey(keybinds.KeyboardDown) ? -1 : 0);
			MovementData.HorizontalMovement = (Input.GetKey(keybinds.KeyboardRight) ? 1 : 0) + (Input.GetKey(keybinds.KeyboardLeft) ? -1 : 0);

			raycastCorrection.y = MovementData.VerticalMovement == -1 ? -1 : 0;
			hit = Physics2D.Raycast(transform.position + raycastCorrection /** MovementData.VerticalMovement*/, Vector2.up * MovementData.VerticalMovement, 1f, LayerMask.GetMask("Environment"));
			Debug.DrawRay(transform.position + raycastCorrection /** MovementData.VerticalMovement*/, Vector2.up * MovementData.VerticalMovement, Color.magenta);

			if (MovementData.VerticalMovement != 0 && hit)
			{
				//if (hit.collider.tag == "Ground")
				//{
					pe = hit.collider.gameObject.GetComponent<PlatformEffector2D>();
					if (pe != null)
					{
						pe.rotationalOffset = MovementData.VerticalMovement == 1 ? 0 : 180;
						canClimb = true;
					}
					//else
					//{
					//	canClimb = false;
					//}
				//}
			}
			//else
			//{
			//	canClimb = false;
			//}

			if (controller.ActiveStateMovement != this && canClimb &&
				(MovementData.VerticalMovement != 0 || (MovementData.HorizontalMovement != 0 && climbing && !(controller.ActiveStateMovement is PlayerJump)))
				&& !rope.RopeAttached)
			{
				if (controller.ActiveStateMovement is PlayerJump)
				{
					controller.ForceSwapState(this);
				}
				else
				{
					//if (/*hit.transform != null ||*/ Input.GetKey(keybinds.KeyboardCrouchKey))
					//{
					controller.SwapState(this);
				}
				//}
			}
		}

		public override void OnEnter_State()
		{
			base.OnEnter_State();
			//pg.enabled = false;
			PlayerGravity.GravityEnabled = false;
			climbing = true;
			if (equipManager.EquippedItem != null && equipManager.EquippedItem.name != "Fists")
			{
				currEquipped = equipManager.EquippedItem;
				equipManager.EquippedItem.Unequip();
			}
		}

		public override void WhileActive_State()
		{
			rigBody.velocity = new Vector2(MovementData.HorizontalMovement * MovementData.MovementSpeed, MovementData.VerticalMovement * MovementData.MovementSpeed);
			if ((MovementData.VerticalMovement == 0 && MovementData.HorizontalMovement == 0) || !canClimb)
			{
				controller.EndState(this);
				return;
			}

			//if (!climbing)
			//{
			//	rigBody.velocity = new Vector2(0f, MovementData.HorizontalMovement * MovementData.MovementSpeed);
			//	if (!(physicsOverlap.Box(new Vector2(transform.position.x, transform.position.y + 1f), 1.5f, 1f).Any(x => x.gameObject.tag == "Climbable")))
			//	{
			//		climbing = true;
			//	}
			//	if (MovementData.HorizontalMovement == 0)
			//	{
			//		controller.EndState(this);
			//		return;
			//	}
			//}
			//else
			//{
			//	rigBody.velocity = new Vector2(0f, MovementData.MovementSpeed);
			//	RaycastHit2D wallHit;
			//	wallHit = Physics2D.Raycast(transform.position + new Vector3(0f, -1f, 0f), -transform.up, 1f, LayerMask.GetMask("Environment"));
			//	Debug.DrawRay(transform.position + new Vector3(0f, -1f, 0f), -transform.up, Color.green);
			//	if (wallHit)
			//	{
			//		rigBody.velocity = new Vector2(0f, 0f);
			//		controller.EndState(this);
			//		return;
			//	}
			//}

		}

		public override void OnExit_State()
		{
			base.OnExit_State();
			//pg.enabled = true;
			PlayerGravity.GravityEnabled = true;
			if (!canClimb)
			{
				climbing = false;
			}

			if (currEquipped != null)
			{
				currEquipped.Equip();
				currEquipped = null;
			}
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.gameObject.tag == "Climbable")
			{
				canClimb = true;
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (collision.gameObject.tag == "Climbable")
			{
				canClimb = false;
			}
		}
	}
}
