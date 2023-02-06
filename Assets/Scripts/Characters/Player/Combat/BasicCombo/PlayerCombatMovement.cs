using System.Collections;
using General.State;
using UnityEngine;

namespace Player.Mechanic.Combat
{
    public class PlayerCombatMovement : StateForMovement
    {
        /// <summary>
        /// Defines spacing for follow up combo.
        /// </summary>
        [SerializeField] protected float followUpComboTime;

        private PlayerIdleCombatStance idleCombatStance { get; set; }

        /// <summary>
        /// Defines follow up attack timer.
        /// </summary>
        protected IEnumerator FollowUpAttackStateTimer { get; set; }

        protected override void Initialization_State()
        {
            base.Initialization_State();
            idleCombatStance = GetComponent<PlayerIdleCombatStance>();
            FollowUpAttackStateTimer = FollowUpAttackState();
        }

        public override void OnEnter_State()
        {
            StopCoroutine(FollowUpAttackStateTimer);
            FollowUpAttackStateTimer = FollowUpAttackState();
        }

        public override void WhileActive_State()
        {
            base.WhileActive_State();
            if (FollowUpAttackStateTimer.Current == null && (controller.ActiveStateMechanic == null))
            {
                StartCoroutine(FollowUpAttackStateTimer);
            }
        }


        public override void OnExit_State()
        {
        }

        /// <summary>
        /// Defines function that will leave space for followup combo.
        /// </summary>
        /// <returns>Yield.</returns>
        protected IEnumerator FollowUpAttackState()
        {
			yield return new WaitForSeconds(followUpComboTime);

			controller.EndState(this);

            if (idleCombatStance != null)
            {
                controller.SwapState(idleCombatStance);
            }
        }
    }
}
