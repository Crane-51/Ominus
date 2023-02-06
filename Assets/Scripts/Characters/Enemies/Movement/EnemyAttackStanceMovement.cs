using General.State;
using UnityEngine;

namespace Enemy.State
{
    public class EnemyAttackStanceMovement : StateForMovement
    {
        protected override void Initialization_State()
        {
            base.Initialization_State();
            Priority = 100;
        }

        public override void OnEnter_State()
        {
        }

        public override void Update_State()
        {
            base.Update_State();
            if (controller.ActiveStateMechanic is EnemyAttackStance && controller.ActiveStateMovement != this)
            {
                controller.ForceSwapState(this);
            }

        }

        public override void WhileActive_State()
        {
            base.WhileActive_State();
            if (!(controller.ActiveStateMechanic is EnemyAttackStance))
            {
                controller.EndState(this);
            }

            rigBody.velocity = new Vector2(0, rigBody.velocity.y);
        }

        public override void OnExit_State()
        {
        }
    }
}
