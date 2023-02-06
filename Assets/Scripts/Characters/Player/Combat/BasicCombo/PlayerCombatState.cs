using System.Collections;
using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;

namespace Player.Mechanic.Combat
{
    public class PlayerCombatState : StateForMechanics
    {
        /// <summary>
        /// Defines range of attack.
        /// </summary>
		[SerializeField] protected float rangeOfAttack;

        /// <summary>
        /// Gets or sets physics overlap.
        /// </summary>
        [InjectDiContainter]
        protected IPhysicsOverlap physicsOverlap { get; set; }

		[FMODUnity.EventRef] [SerializeField] protected string targetHitEvent;

		protected int bonusDmg;
		protected PlayerCombatController pcc;

		protected override void Initialization_State()
		{
			base.Initialization_State();
			pcc = GetComponent<PlayerCombatController>();
		}

		/// <summary>
		/// Defines cooldown of the attack. Attack is over when animation ends.
		/// </summary>
		/// <returns>Yield.</returns>
		protected IEnumerator AttackColdown()
        {
			// if it starts making problems, switch to clip.length
            yield return new WaitUntil(() => { return designController.animationController.IsAnimationOver(this); });
            controller.EndState(this);
        }
    }
}
