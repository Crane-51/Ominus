using System;
using System.Collections;
using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using Player.Movement;
using Player.Sneak;
using UnityEngine;

namespace Enemy.State
{
	[RequireComponent(typeof(EnemyFollowTarget))]
	public class EnemyDetectTarget : StateForMechanics
	{
		/// <summary>
		/// Gets or sets enemy data.
		/// </summary>
		[InjectDiContainter]
		private IGameInformation gameInformation { get; set; }

		private EnemyInvestigateMovement enemyInvestigateMovement { get; set; }
		private EnemySharedDataAndInit sharedData;

		protected override void Initialization_State()
		{
			base.Initialization_State();
			Priority = 6;
			enemyInvestigateMovement = GetComponent<EnemyInvestigateMovement>();
			sharedData = GetComponent<EnemySharedDataAndInit>();
			sharedData.targetLocked = false;
		}

		public override void OnEnter_State()
		{
			base.OnEnter_State();
			sharedData.targetLocked = true;
			//if (!TargetLocked)
			//{
			//	StartCoroutine(TimeBeforeDetection());
			//}
		}

		private IEnumerator TimeBeforeDetection()
		{
			yield return new WaitForSeconds(2);

			if (gameInformation.PlayerStateController.ActiveStateMovement is PlayerSneakIdle)
			{
				controller.EndState(this);
			}
			else
			{
				sharedData.targetLocked = true;
			}
		}

		public override void Update_State()
		{
			base.Update_State();

			if (sharedData.targetLocked)
			{
				sharedData.lastKnownPlayerPosition = gameInformation.Player.transform.position;
			}

			if (!(controller.ActiveHighPriorityState is Character.Stats.CharacterIsDead))
			{
				sharedData.targetInRangeOfVision = sharedData.enemyData.IsTargetStillInRangeOfVision(transform, gameInformation.Player.transform);
			}

			if (controller.ActiveStateMechanic != this && !(controller.ActiveHighPriorityState is Character.Stats.CharacterIsDead))
			{				
				if (sharedData.targetInRangeOfVision)
				{
					if (!sharedData.targetLocked &&
						(
							gameInformation.PlayerStateController.ActiveStateMovement is StateForMovement
							&&
							(!(gameInformation.PlayerStateController.ActiveStateMovement is PlayerSneakIdle)
							&& !(gameInformation.PlayerStateController.ActiveStateMovement is PlayerSneak)
							&& !(gameInformation.PlayerStateController.ActiveStateMovement is PlayerIdle))
							||
							((gameInformation.PlayerStateController.ActiveStateMovement is PlayerSneak
							|| gameInformation.PlayerStateController.ActiveStateMovement is PlayerSneakIdle
							|| gameInformation.PlayerStateController.ActiveStateMovement is PlayerIdle)
							&& sharedData.enemyData.NotSneakingRangeOfDetection(transform, gameInformation.Player.transform))
						))
					{
						controller.SwapState(this);
					}
				}
				else
				{
					sharedData.targetLocked = false;
				}
			}
		}

		public override void WhileActive_State()
		{
			sharedData.enemyData.LookAtTarget(transform, gameInformation.Player.transform);

			if (!sharedData.targetInRangeOfVision)
			{
				sharedData.targetLocked = false;
				controller.EndState(this);
			}
		}

		public override void OnExit_State()
		{
			designController.animationController.StopAnimation(this.GetType().Name);
			if (!(controller.ActiveStateMovement is EnemyInvestigateMovement) && !sharedData.targetLocked)
			{
				controller.ForceSwapState(enemyInvestigateMovement);

			}
		}
	}
}
