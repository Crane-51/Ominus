using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;

namespace Enemy.State
{
	public class EnemyInvestigate : StateForMechanics
	{
		private EnemyInvestigateMovement eim;

		protected override void Initialization_State()
		{
			base.Initialization_State();
			Priority = 5;//todo check
			eim = GetComponent<EnemyInvestigateMovement>();
		}

		public override void Update_State()
		{
			base.Update_State();
			if (controller.ActiveStateMechanic != this && controller.ActiveStateMovement is EnemyInvestigateMovement)
			{
				controller.SwapState(this);
			}
		}

		public override void WhileActive_State()
		{
			base.WhileActive_State();
			if (!(controller.ActiveStateMovement is EnemyInvestigateMovement))
			{
				controller.EndState(this);
			}
		}

		public override void OnExit_State()
		{
			base.OnExit_State();
			controller.EndState(eim);
		}
	}
}