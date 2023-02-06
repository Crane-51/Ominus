using System.Collections.Generic;
using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;

namespace Enemy.State
{
	public class EnemyCombatController : StateForMechanics
	{
		/// <summary>
		/// Defines list of combo attacks.
		/// </summary>
		[SerializeField] private List<General.State.State> AttackStates;

		/// <summary>
		/// Gets or sets combo index.
		/// </summary>
		private int ComboIndex { get; set; }

		/// <summary>
		/// Gets or sets last active state for mechanic.
		/// </summary>
		private StateForMovement lastStateForMovement { get; set; }

		/// <summary>
		/// Gets or sets enemy data.
		/// </summary>
		[InjectDiContainter]
		private IGameInformation gameInformation { get; set; }

		private EnemySharedDataAndInit sharedData;

		protected override void Initialization_State()
		{
			base.Initialization_State();
			Priority = 15;
			ComboIndex = 0;
			sharedData = GetComponent<EnemySharedDataAndInit>();
		}

		public override void OnEnter_State()
		{
			if (!(lastStateForMovement is EnemyAttackMovement))
			//if (!GetComponent<EnemyAttackMovement>().waitingFollowup)
			{
				ComboIndex = 0;
			}
			controller.SwapState(AttackStates[ComboIndex % AttackStates.Count]);
			ComboIndex++;
		}

		public override void Update_State()
		{
			base.Update_State();

			lastStateForMovement = controller.ActiveStateMovement;
			if (controller.ActiveStateMechanic != this && !(controller.ActiveStateMovement is EnemySleep) && sharedData.enemyData.CanAttack && !(controller.ActiveHighPriorityState is Character.Stats.CharacterIsDead))
			{
				if (sharedData.targetLocked && sharedData.enemyData.IsTargetInRangeOfMeleeAttack(transform, gameInformation.Player.transform))
				{
					controller.SwapState(this);
				}
				//if (controller.ActiveStateMovement is EnemyFollowTarget)
				//{
				//	if (enemyData.IsTargetInRangeOfMeleeAttack(transform, gameInformation.Player.transform))
				//	{
				//		controller.SwapState(this);
				//	}
				//}
				//else if (controller.ActiveStateMechanic is EnemyAttackStance)
				//{
				//	controller.SwapState(this);
				//}

			}
		}

		public override void OnExit_State()
		{
		}
	}
}
