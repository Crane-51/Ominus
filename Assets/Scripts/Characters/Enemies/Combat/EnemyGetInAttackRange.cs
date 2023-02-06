using System;
using System.Collections;
using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;

namespace Enemy.State
{
	public class EnemyGetInAttackRange : StateForMechanics
	{
		protected override void Initialization_State()
		{
			base.Initialization_State();
			Priority = 8;
		}

		public override void Update_State()
		{
			base.Update_State();
			if (controller.ActiveStateMechanic != this && (controller.ActiveStateMovement is EnemyFollowTarget || controller.ActiveStateMovement is EnemyRunAway))
			{
				controller.SwapState(this);
			}
		}

		public override void WhileActive_State()
		{
			if (!(controller.ActiveStateMovement is EnemyFollowTarget || controller.ActiveStateMovement is EnemyRunAway))
			{				
				controller.EndState(this);
			}
		}
	}
}