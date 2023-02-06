using UnityEngine;
using Player.Other;
using General.State;
using DiContainerLibrary.DiContainer;
using Implementation.Data;

namespace Player.Movement
{
	/// <summary>
	/// Class that defines state of grabing ledges.
	/// </summary>
	public class PlayerGrabLedge : StateForMovement
	{
		[SerializeField] private float detectionDistance = 0.7f;
		[SerializeField] private float ledgeDetectionCorrectionDistance = 0.2f;
		[SerializeField] private Vector2 ledgeDetectionCorrectionY = new Vector2(0f, 0.2f);
		[SerializeField] private float detectionOriginOffsetY = 0.7f;
		[SerializeField] private bool debugging = false;
		private float nextGrabDelay = 0.5f;
		private float nextGrabTimer = 0f;
		protected EquipmentManager equipManager;
		protected Equipment currEquipped;
		[InjectDiContainter]
		protected IPlayerKeybindsData keybinds;

		protected override void Initialization_State()
		{
			base.Initialization_State();
			Priority = 11;
			equipManager = GetComponent<EquipmentManager>();
			keybinds = SaveAndLoadData<IPlayerKeybindsData>.LoadSpecificData("Keybinds");
		}

		public override void OnEnter_State()
		{
			base.OnEnter_State();
			rigBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
			if (equipManager.EquippedItem != null && equipManager.EquippedItem.name != "Fists")
			{
				currEquipped = equipManager.EquippedItem;
				equipManager.EquippedItem.Unequip();
			}
		}

		public override void Update_State()
		{
			if (nextGrabTimer < nextGrabDelay)
			{
				nextGrabTimer += Time.deltaTime;
			}

			if (debugging)
			{
				Vector2 detectionOrigin = new Vector2(transform.position.x, transform.position.y + detectionOriginOffsetY);
				Debug.DrawRay(detectionOrigin + ledgeDetectionCorrectionY, Quaternion.AngleAxis(transform.localScale.x * 15, Vector3.forward) * transform.right * transform.localScale.x * (detectionDistance + ledgeDetectionCorrectionDistance), Color.green);
				Debug.DrawRay(detectionOrigin, Quaternion.AngleAxis(transform.localScale.x * -15, Vector3.forward) * transform.right * transform.localScale.x * detectionDistance, Color.red);

				RaycastHit2D ceilingHit =
					Physics2D.Raycast(transform.position, Vector2.up, detectionOriginOffsetY + ledgeDetectionCorrectionY.y, LayerMask.GetMask("Environment"));
				Debug.DrawRay(transform.position, Vector2.up * (detectionOriginOffsetY + ledgeDetectionCorrectionY.y), Color.magenta);
			}
			

			//if (PlayerGravity.CollisionCount == 0 && rigBody.velocity.y <= 0 && controller.ActiveStateMovement != this && nextGrabTimer >= nextGrabDelay && !ceilingHit.collider)
			if (!PlayerGravity.IsGrounded && rigBody.velocity.y <= 0 && controller.ActiveStateMovement != this && nextGrabTimer >= nextGrabDelay 
				&& !(controller.ActiveStateMovement is PlayerClimbIdle) && !(controller.ActiveStateMovement is PlayerClimbMovement))
			{
				RaycastHit2D ceilingHit =
					Physics2D.Raycast(transform.position, Vector2.up, detectionOriginOffsetY + ledgeDetectionCorrectionY.y, LayerMask.GetMask("Environment"));

				if (!ceilingHit.collider)
				{
					Vector2 detectionOrigin = new Vector2(transform.position.x, transform.position.y + detectionOriginOffsetY);

					RaycastHit2D ledgeHit;
					RaycastHit2D wallHit;

					ledgeHit = Physics2D.Raycast(detectionOrigin + ledgeDetectionCorrectionY, Quaternion.AngleAxis(transform.localScale.x * 15, Vector3.forward) * transform.right * transform.localScale.x, (detectionDistance + ledgeDetectionCorrectionDistance), LayerMask.GetMask("Environment"));
					wallHit = Physics2D.Raycast(detectionOrigin, Quaternion.AngleAxis(transform.localScale.x * (-15), Vector3.forward) * transform.right * transform.localScale.x, detectionDistance, LayerMask.GetMask("Environment"));

					if (!ledgeHit.collider && wallHit.collider)
					{
						controller.SwapState(this);
					} 
				}
			}
		}

		public override void WhileActive_State()
		{
			if (Input.GetKey(keybinds.KeyboardDown))
			{
				//just in case, remove y constraint?
				controller.EndState(this);
			}
		}
		
		public override void OnExit_State()
		{
			base.OnExit_State();
			nextGrabTimer = 0f;
			if (currEquipped != null)
			{
				currEquipped.Equip();
				currEquipped = null;
			}
		}
	}
}