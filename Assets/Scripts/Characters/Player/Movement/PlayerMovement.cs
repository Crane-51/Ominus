using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;
using Player.Other;

namespace Player.Movement
{
	[RequireComponent(typeof(PlayerJump))]
	public class PlayerMovement : StateForMovement
	{
		/// <summary>
		/// Gets or sets player key binds;
		/// </summary>
		[InjectDiContainter]
		protected IPlayerKeybindsData keybinds { get; set; }
		private CapsuleCollider2D capsule;
		private Vector3 checkPosition;
		[SerializeField] private float checkDistance = 1.5f;
		private Vector2 slopeDirection;
		private float slopeAngle = 0f;

		protected override void Initialization_State()
		{
			base.Initialization_State();
			Priority = -5;
			keybinds = SaveAndLoadData<IPlayerKeybindsData>.LoadSpecificData("Keybinds");
			capsule = GetComponent<CapsuleCollider2D>();
		}

		public override void Update_State()
		{
			MovementData.HorizontalMovement = (Input.GetKey(keybinds.KeyboardRight) ? 1 : 0) + (Input.GetKey(keybinds.KeyboardLeft) ? -1 : 0);

			if (MovementData.HorizontalMovement != 0)
			{
				controller.SwapState(this);
			}
		}

		public override void WhileActive_State()
		{
			if (MovementData.HorizontalMovement == 0)
			{
				rigBody.velocity = new Vector2(0, rigBody.velocity.y);
				controller.EndState(this);
				return;
			}

			rigBody.gameObject.transform.localScale = new Vector3(MovementData.HorizontalMovement, 1, 1);

			RaycastHit2D enemyHit = Physics2D.Raycast(transform.position, transform.localScale.x * transform.right, 1.5f, LayerMask.GetMask("Enemy"));
			Debug.DrawRay(transform.position, transform.localScale.x * transform.right * 1.5f, Color.green);

			if (!enemyHit)
			{
				// Slope check
				checkPosition = transform.position - new Vector3(0f, capsule.size.y / 2);
				RaycastHit2D hit = Physics2D.Raycast(checkPosition, Vector2.down, checkDistance, LayerMask.GetMask("Environment"));
				if (hit)
				{
					Debug.DrawRay(checkPosition, Vector2.down, Color.yellow);
					slopeDirection = Vector2.Perpendicular(hit.normal).normalized;
					Debug.DrawRay(checkPosition, slopeDirection, Color.white);
					slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
				}

				// Standard non-slope movement
				if (slopeAngle == 0)
				{
					rigBody.velocity = new Vector2(MovementData.HorizontalMovement * MovementData.MovementSpeed, rigBody.velocity.y);
				}
				// Player on slope
				else if (PlayerGravity.IsGrounded)
				{
					rigBody.velocity = new Vector2(-MovementData.HorizontalMovement * MovementData.MovementSpeed * slopeDirection.x, -MovementData.HorizontalMovement * MovementData.MovementSpeed * slopeDirection.y);
				}
				// Player in air
				else
				{
					rigBody.velocity = new Vector2(-MovementData.HorizontalMovement * MovementData.MovementSpeed * slopeDirection.x, rigBody.velocity.y);
				} 
			}
			else
			{
				rigBody.velocity = new Vector2(0f, rigBody.velocity.y);
			}
		}
	}
}
