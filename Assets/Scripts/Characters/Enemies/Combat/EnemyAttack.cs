using System.Collections;
using Character.Stats;
using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;

namespace Enemy.State
{
    public class EnemyAttack : StateForMechanics
    {
        /// <summary>
        /// Defines enemy combat range buff.
        /// </summary>
        private const float RangeBuff = 2f;

        /// <summary>
        /// Gets or sets enemy data.
        /// </summary>
        [InjectDiContainter]
        private IGameInformation gameInformation { get; set; }

		/// <summary>
		/// Gets or sets player stats.
		/// </summary>
		private CharacterTakeDamage target { get; set; }

        /// <summary>
        /// Gets or sets attack cooldown timer.
        /// </summary>
        private IEnumerator attackCooldownTimer { get; set; }
		private EnemySharedDataAndInit sharedData;

		[SerializeField] private float impactForce = 1f;

        protected override void Initialization_State()
        {
            base.Initialization_State();
            Priority = 15;
            target = gameInformation.Player.GetComponent<CharacterTakeDamage>();
			sharedData = GetComponent<EnemySharedDataAndInit>();
        }

        public override void OnEnter_State()
        {
            base.OnEnter_State();
			sharedData.enemyData.MinRangeOfAttack *= RangeBuff;
			sharedData.enemyData.CanAttack = false;
            attackCooldownTimer = AttackCooldown();

			int dmg = DiceRoller.RollDieWithModifier(sharedData.weaponData.DieToRoll, sharedData.weaponData.Type == General.Enums.WeaponType.Melee ? sharedData.enemyStats.Strength : sharedData.enemyStats.Dexterity);
			target.TakeDamage(dmg, transform.localScale.x, impactForce);
            StartCoroutine(attackCooldownTimer);
        }

        public override void Update_State()
        {
            base.Update_State();
            if (sharedData.enemyData.CanAttack && !(controller.ActiveHighPriorityState is CharacterIsDead))
            {
                if (controller.ActiveStateMovement is EnemyFollowTarget)
                {
                    if (sharedData.enemyData.IsTargetInRangeOfMeleeAttack(transform, gameInformation.Player.transform))
                    {
                        controller.SwapState(this);
                    }
                }
                else if (controller.ActiveStateMechanic is EnemyAttackStance)
                {
                    controller.SwapState(this);
                }
            }
        }

        public override void OnExit_State()
        {
            base.OnExit_State();
			sharedData.enemyData.MinRangeOfAttack /= RangeBuff;
        }

        private IEnumerator AttackCooldown()
        {
            yield return new WaitUntil(() => designController.animationController.IsAnimationOver(this));
            controller.EndState(this);
            yield return new WaitForSeconds(sharedData.enemyData.AttackCooldown);
			sharedData.enemyData.CanAttack = true;
        }
    }
}
