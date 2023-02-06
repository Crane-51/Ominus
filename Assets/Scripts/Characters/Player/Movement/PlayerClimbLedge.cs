using UnityEngine;
using DiContainerLibrary.DiContainer;
using Implementation.Data;
using System.Linq;
using Player.Other;
using General.State;

namespace Player.Movement
{
	public class PlayerClimbLedge : StateForMovement
	{
		[InjectDiContainter]
		protected IPlayerKeybindsData keybinds { get; set; }
		/// <summary>
		/// Gets or sets physics overlap.
		/// </summary>
		[InjectDiContainter]
		private IPhysicsOverlap physicsOverlap { get; set; }
		private Vector2 offsetToTheTop = new Vector2(0.8f, 1.8f);
		private float length = 0.8f;
		private float width = 1.6f;
		private Vector2 vertical;
		private Vector2 horizontal;
		private PlayerGravity pGravity;
		protected EquipmentManager equipManager;
		protected Equipment currEquipped;
		private float startingX;

		[SerializeField] private Vector2 frontDetectOffset = new Vector2(1f, -1f);
		[SerializeField] private float frontDetectWidth = 1f;
		[SerializeField] private float frontDetectHeight = 2f;
		[SerializeField] private Vector2 climbingSpeed = new Vector2(3f, 3f);
		[SerializeField] private Vector2 bottomDetectOffset = new Vector2(0.5f, -1f);
		[SerializeField] private float bottomDetectDepth = 1f;

		protected override void Initialization_State()
		{
			base.Initialization_State();
			Priority = 12;
			keybinds = SaveAndLoadData<IPlayerKeybindsData>.LoadSpecificData("Keybinds");
			pGravity = GetComponent<PlayerGravity>();
			equipManager = GetComponent<EquipmentManager>();

		}

		public override void OnEnter_State()
		{
			base.OnEnter_State();
			rigBody.constraints = RigidbodyConstraints2D.FreezeRotation;
			//pGravity.enabled = false;
			PlayerGravity.GravityEnabled = false;
			if (equipManager.EquippedItem != null && equipManager.EquippedItem.name != "Fists")
			{
				currEquipped = equipManager.EquippedItem;
				equipManager.EquippedItem.Unequip();
			}			
		}

		public override void Update_State()
		{
			if ((controller.ActiveStateMovement is PlayerGrabLedge) && Input.GetKey(keybinds.KeyboardUp)
				&& !physicsOverlap.Box(new Vector2(transform.position.x + (offsetToTheTop.x * transform.localScale.x), transform.position.y + offsetToTheTop.y), length, width).Any(x => x.gameObject.tag == "Ground"))
			{
				//horizontal = new Vector2(transform.position.x + ((offsetToTheTop.x + 1f) * transform.localScale.x), transform.position.y + offsetToTheTop.y + 1f);
				//vertical = new Vector2(transform.position.x, transform.position.y + offsetToTheTop.y + 1f);
				controller.SwapState(this);
			}
		}

		public override void WhileActive_State()
		{
			// THIS WORKS ON STATIC CLIMBABLE OBJECTS
			//if (transform.position.y < vertical.y - 1f)
			//{
			//	transform.position = Vector2.Lerp(transform.position, vertical, Time.deltaTime * 2.5f);
			//	Debug.DrawLine(vertical, horizontal, Color.cyan, 10);
			//}
			//else
			//{
			//	if (Mathf.Abs(transform.position.x - horizontal.x) >= 1f && !designController.animationController.IsAnimationOver(this))
			//	{
			//		transform.position = Vector2.Lerp(transform.position, horizontal, Time.deltaTime * 1.5f);

			//	}
			//	else
			//	{
			//		controller.EndState(this);
			//	}
			//}
			// END OF THIS WORKS ON STATIC

			//shoot ray/overlap in front -> if wall then upward force else y = 0
			//if no wall in front, shoot ray below -> if null then horizontal force else end state
			if (physicsOverlap.Box(new Vector2(transform.position.x + (frontDetectOffset.x * transform.localScale.x), transform.position.y + frontDetectOffset.y), frontDetectWidth, frontDetectHeight).Any(x => x.gameObject.tag == "Ground"))
			{
				rigBody.velocity = new Vector2(rigBody.velocity.x, climbingSpeed.y);
				startingX = transform.position.x;
			}
			else
			{
				rigBody.velocity = new Vector2(rigBody.velocity.x, 0f);
				RaycastHit2D wallHit;
				wallHit = Physics2D.Raycast(transform.position + new Vector3(bottomDetectOffset.x * -transform.localScale.x, bottomDetectOffset.y, 0f), -transform.up, bottomDetectDepth, LayerMask.GetMask("Environment"));
				Debug.DrawRay(transform.position + new Vector3(bottomDetectOffset.x * -transform.localScale.x, bottomDetectOffset.y, 0f), -transform.up * bottomDetectDepth, Color.green);
				if (wallHit || Mathf.Abs(transform.position.x - startingX) > 1.5)
				{
					controller.EndState(this);
				}
				else
				{
					rigBody.velocity = new Vector2(climbingSpeed.x * transform.localScale.x, 0f);
				}
			}

		}


		public override void OnExit_State()
		{
			base.OnExit_State();
			//pGravity.enabled = true;
			PlayerGravity.GravityEnabled = true;
			//transform.position = new Vector2(transform.position.x + (offsetToTheTop.x * transform.localScale.x), transform.position.y + offsetToTheTop.y);
			if (currEquipped != null)
			{
				currEquipped.Equip();
				currEquipped = null;
			}
		}
	}
}
