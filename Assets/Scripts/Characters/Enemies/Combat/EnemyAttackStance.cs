using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;

namespace Enemy.State
{
    public class EnemyAttackStance : StateForMechanics
    {
        /// <summary>
        /// Gets or sets enemy data.
        /// </summary>
        [InjectDiContainter]
        private IGameInformation gameInformation { get; set; }
		private EnemyInvestigateMovement eim;
		private EnemySharedDataAndInit sharedData;

        protected override void Initialization_State()
        {
            base.Initialization_State();
            Priority = 14;
			eim = GetComponent<EnemyInvestigateMovement>();
			sharedData = GetComponent<EnemySharedDataAndInit>();
		}

        public override void OnEnter_State()
        {
            base.OnEnter_State();
            //enemyData.MinRangeOfAttack *= 2;
        }

        public override void Update_State()
        {
            base.Update_State();
			if (sharedData.targetLocked && !sharedData.enemyData.CanAttack && !(controller.ActiveHighPriorityState is Character.Stats.CharacterIsDead))
            {
                if (sharedData.enemyData.IsTargetInRangeOfMeleeAttack(transform, gameInformation.Player.transform))
                {
                    controller.SwapState(this);
                }
            }
        }


        public override void WhileActive_State()
        {
			sharedData.enemyData.LookAtTarget(transform, gameInformation.Player.transform);

            if (!sharedData.enemyData.IsTargetInRangeOfMeleeAttack(transform, gameInformation.Player.transform))
            {
				if (!sharedData.targetLocked)
				{
					controller.ForceSwapState(eim);
				}

				controller.EndState(this);

			}
		}

        public override void OnExit_State()
        {
            base.OnExit_State();
            //enemyData.MinRangeOfAttack /= 2;
        }
    }
}
