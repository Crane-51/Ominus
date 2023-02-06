using System.Linq;
using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;

namespace Enemy.State
{
	public class EnemyRunAway : StateForMovement
	{
		/// <summary>
		/// Gets or sets enemy data.
		/// </summary>
		[InjectDiContainter]
		private IGameInformation gameInformation { get; set; }

		/// <summary>
		/// Gets or sets physics overlap.
		/// </summary>
		[InjectDiContainter]
		private IPhysicsOverlap physicsOverlap { get; set; }
		private EnemySharedDataAndInit sharedData;
		private EnemyInvestigateMovement eim;

		protected override void Initialization_State()
		{
			base.Initialization_State();
			Priority = 7;
			sharedData = GetComponent<EnemySharedDataAndInit>();
			eim = GetComponent<EnemyInvestigateMovement>();
			sharedData.forceAim = false;
		}

		public override void WhileActive_State()
		{
			base.WhileActive_State();

			if (!IsInsideMinRange() || controller.ActiveStateMechanic is EnemyAiming)
			{
				if (!sharedData.targetLocked && !(controller.ActiveStateMovement is EnemyInvestigateMovement))
				{
					controller.ForceSwapState(eim);
				}
				controller.EndState(this);
			}
			else
			{
				RaycastHit2D wallHit =
					Physics2D.Raycast(transform.position, transform.localScale.x == -1? Vector3.left : Vector3.right, 1.5f, LayerMask.GetMask("Environment", "Border"));
				Debug.DrawRay(transform.position, (transform.localScale.x == -1 ? Vector3.left : Vector3.right) * 1.5f, Color.magenta);
				if (wallHit.collider)
				{
					sharedData.forceAim = true;
					if (!sharedData.targetLocked && !(controller.ActiveStateMovement is EnemyInvestigateMovement))
					{
						controller.ForceSwapState(eim);
					}
					controller.EndState(this);
				}
				else
				{
					rigBody.velocity = new Vector2(MovementData.MovementSpeed * sharedData.enemyData.LookAwayFromTarget(transform, gameInformation.Player.transform), rigBody.velocity.y);
				}

			}
		}


		public override void Update_State()
		{
			if (controller.ActiveStateMovement != this && sharedData.targetLocked && !(controller.ActiveHighPriorityState is Character.Stats.CharacterIsDead)
				&& IsInsideMinRange())
			{
				if (sharedData.forceAim)
				{
					RaycastHit2D wallHitLeft =
						Physics2D.Raycast(transform.position, Vector3.left, 1.5f, LayerMask.GetMask("Environment", "Border"));
					Debug.DrawRay(transform.position, Vector3.left * 1.5f, Color.magenta);

					RaycastHit2D wallHitRight =
						Physics2D.Raycast(transform.position, Vector3.right, 1.5f, LayerMask.GetMask("Environment", "Border"));
					Debug.DrawRay(transform.position, Vector3.right * 1.5f, Color.magenta);
					if (!wallHitLeft.collider && !wallHitRight.collider)
					{
						sharedData.forceAim = false;
						controller.SwapState(this);
					}
				}
				else
				{
					controller.SwapState(this);
				}				
			}
		}

		// If starts making problems -> raycast
		private bool IsInsideMinRange()
		{
			return Vector2.Distance(gameInformation.Player.transform.position, transform.position) < sharedData.enemyData.MinRangeOfAttack;
		}
	}
}