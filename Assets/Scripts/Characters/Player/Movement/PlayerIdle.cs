using General.State;
using UnityEngine;

namespace Player.Movement
{
    public class PlayerIdle : StateForMovement
    {
        protected override void Initialization_State()
        {
            base.Initialization_State();
            Priority = -10;
        }

        public override void OnEnter_State()
        {
            base.OnEnter_State();
            rigBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }

        public override void Update_State()
        {
            if(controller.ActiveStateMovement == null)
            {
                controller.SwapState(this);
            }
        }

        public override void OnExit_State()
        {
            base.OnExit_State();
            rigBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        public override void WhileActive_State()
        {
            base.WhileActive_State();
            rigBody.velocity = new Vector2(0, rigBody.velocity.y);
        }
    }
}
