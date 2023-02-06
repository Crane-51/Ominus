using General.State;
using UnityEngine;

namespace Enemy.State
{
    public class EnemyIdle : StateForMovement
    {
		private EnemySharedDataAndInit sharedData;

        protected override void Initialization_State()
        {
            base.Initialization_State();
            Priority = -15;
			sharedData = GetComponent<EnemySharedDataAndInit>();
        }

        public override void Update_State()
        {
            base.Update_State();

            if(controller.ActiveStateMovement == null && controller.ActiveStateMechanic == null && (!sharedData.targetLocked))
            {
                controller.SwapState(this);
            }
        }

        public override void WhileActive_State()
        {
            base.WhileActive_State();
            rigBody.velocity = new Vector2(0, rigBody.velocity.y);
        }
    }
}
