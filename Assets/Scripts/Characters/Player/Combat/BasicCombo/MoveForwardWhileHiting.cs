using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Player.Mechanic.Combat
{
    public class MoveForwardWhileHiting : PlayerCombatMovement
    {
        /// <summary>
        /// Defines states that use this movement mechanic and movement speed that is used for it.
        /// </summary>
        private Dictionary<Type, float> stateThatUseThisMovement = new Dictionary<Type, float>()
        {
            {typeof(RightPunch), 0.3f },
            {typeof(LeftPunch), 0.3f },
            {typeof(LegKick), 0.6f }
        };

		private bool waitingFollowup = false;

        protected override void Initialization_State()
        {
            base.Initialization_State();
            Priority = 100;
        }

        public override void Update_State()
        {
            base.Update_State();

            if(controller.ActiveStateMechanic != null && stateThatUseThisMovement.Keys.Contains(controller.ActiveStateMechanic.GetType()))
            {
                controller.SwapState(this);
            }
        }

        public override void WhileActive_State()
        {
            base.WhileActive_State();

            if(!stateThatUseThisMovement.Keys.Contains(this.GetType()) && !waitingFollowup)
            {
                StartCoroutine(FollowUpAttackStateTimer);
				designController.animationController.Anima.SetBool("PlayerIdleCombatStance", true);
				waitingFollowup = true;
            }

			RaycastHit2D enemyHit = Physics2D.Raycast(transform.position, transform.localScale.x * transform.right, 1.5f, LayerMask.GetMask("Enemy"));
			Debug.DrawRay(transform.position, transform.localScale.x * transform.right * 1.5f, Color.green);

            if (controller.ActiveStateMechanic != null && !enemyHit)
            {	
				rigBody.velocity = new Vector2(rigBody.transform.localScale.x * stateThatUseThisMovement[controller.ActiveStateMechanic.GetType()], rigBody.velocity.y); 				
            }
			else
			{
				rigBody.velocity = new Vector2(0, rigBody.velocity.y);
			}
        }

		public override void OnExit_State()
		{
			base.OnExit_State();
			waitingFollowup = false;
			designController.animationController.Anima.SetBool("PlayerIdleCombatStance", false);
		}
	}
}
