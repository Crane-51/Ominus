using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Enemy.State
{
    public class EnemyAttackMovement : StateForMovement
    {
		private List<Type> statesThatUseThisMovement = new List<Type> { typeof(EnemyComboAttack), typeof(EnemyFinalAttack), typeof(EnemyAttack) };	
		private bool waitingFollowup = false;		
		[SerializeField] private float followUpComboTime;
		[SerializeField] private float fwdSpeed = 0.5f;
		private EnemyAttackStance attackStance;

		protected override void Initialization_State()
        {
            base.Initialization_State();
            Priority = 101;
			attackStance = GetComponent<EnemyAttackStance>();
		}

        public override void OnEnter_State()
        {
			StopCoroutine(FollowUpAttackState());
		}

        public override void Update_State()
        {
            base.Update_State();
            if (controller.ActiveStateMovement != this && controller.ActiveStateMechanic != null && statesThatUseThisMovement.Contains(controller.ActiveStateMechanic.GetType()))
            {
                controller.SwapState(this);
            }
        }

        public override void WhileActive_State()
        {
            base.WhileActive_State();

			if (controller.ActiveStateMechanic == null || !statesThatUseThisMovement.Contains(controller.ActiveStateMechanic.GetType()))
			{
				rigBody.velocity = new Vector2(0, rigBody.velocity.y);

				if (!waitingFollowup)
				{
					StartCoroutine(FollowUpAttackState());
					designController.animationController.Anima.SetBool("EnemyAttackStance", true);
					waitingFollowup = true;
				}			
			}
			else
			{
				rigBody.velocity = new Vector2(rigBody.transform.localScale.x * fwdSpeed, rigBody.velocity.y);
				if (waitingFollowup)
				{
					waitingFollowup = false;
					StopCoroutine(FollowUpAttackState());
					designController.animationController.Anima.SetBool("EnemyAttackStance", false);
				}
				
			}

			if (FollowUpAttackState().Current == null && (controller.ActiveStateMechanic == null) && !waitingFollowup)
			{
				StartCoroutine(FollowUpAttackState());
				waitingFollowup = true;
			}
		}

        public override void OnExit_State()
        {
			base.OnExit_State();
			waitingFollowup = false;
			if (!(controller.ActiveStateMechanic is EnemyAttackStance))
			{
				designController.animationController.Anima.SetBool("EnemyAttackStance", false);

			}
		}

		/// <summary>
		/// Defines function that will leave space for followup combo.
		/// </summary>
		/// <returns>Yield.</returns>
		private IEnumerator FollowUpAttackState()
		{
			yield return new WaitForSeconds(followUpComboTime);

			controller.EndState(this);

			if (attackStance != null)
			{
				controller.SwapState(attackStance);
			}
			
			waitingFollowup = false;
		}
	}
}
