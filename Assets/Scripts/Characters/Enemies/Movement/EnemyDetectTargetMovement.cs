using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;

namespace Enemy.State
{
    public class EnemyDetectTargetMovement : StateForMovement
    {
        /// <summary>
        /// Gets or sets enemy data.
        /// </summary>
        [InjectDiContainter]
        private IGameInformation gameInformation { get; set; }
		private EnemySharedDataAndInit sharedData;

        protected override void Initialization_State()
        {
            base.Initialization_State();
            Priority = 6;
			sharedData = GetComponent<EnemySharedDataAndInit>();
        }

        public override void OnEnter_State()
        {
        }

        public override void Update_State()
        {
            base.Update_State();

            if (controller.ActiveStateMovement != this && controller.ActiveStateMechanic is EnemyDetectTarget)
            {
                controller.SwapState(this);
            }
        }

        public override void WhileActive_State()
        {
            base.WhileActive_State();
            sharedData.enemyData.LookAtTarget(transform, gameInformation.Player.transform);

            rigBody.velocity = new Vector2(0 , rigBody.velocity.y);

            if (!(controller.ActiveStateMechanic is EnemyDetectTarget))
            {
                controller.EndState(this);
            }
        }

        public override void OnExit_State()
        {
        }
    }
}
