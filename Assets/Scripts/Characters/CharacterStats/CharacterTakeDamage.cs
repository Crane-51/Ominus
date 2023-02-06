using System.Collections;
using General.State;
using UnityEngine;

namespace Character.Stats
{
    public class CharacterTakeDamage : HighPriorityState
    {
        /// <summary>
        /// Gets or sets can get damage component.
        /// </summary>
        private bool CanTakeDamage { get; set; }

        /// <summary>
        /// Gets or sets immune to damage duration.
        /// </summary>
        [SerializeField] private float ImmuneToDamageDuration;

        /// <summary>
        /// Gets or sets rigidbody component.
        /// </summary>
        private Rigidbody2D rigBody { get; set; }

        /// <summary>
        /// Gets or sets character stats.
        /// </summary>
        private CharacterStatsMono CharacterStats { get; set; }

        /// <summary>
        /// Direction of hit.
        /// </summary>
        private float direction { get; set; }

		/// <summary>
		/// Force of hit
		/// </summary>
		private float force { get; set; }

		private float initialImmunity;
		[SerializeField] private bool immuneToAttackDmg = false;
		protected EquipmentManager equipManager;
		protected Equipment currEquipped;

		protected override void Initialization_State()
        {
            base.Initialization_State();
            rigBody = GetComponent<Rigidbody2D>();
			Priority = 200;
			CharacterStats = GetComponent<CharacterStatsMono>();
            CanTakeDamage = true;
			initialImmunity = ImmuneToDamageDuration;
			if (tag == "Player")
			{
				equipManager = GetComponent<EquipmentManager>(); 
			}
		}

        public override void OnEnter_State()
        {
            base.OnEnter_State();
            if (direction != 0)
            {
                transform.localScale = new Vector3(-direction, 1, 1);
            }
            StartCoroutine(stateDuration());

			if (equipManager != null && equipManager.EquippedItem != null && equipManager.EquippedItem.name != "Fists")
			{
				currEquipped = equipManager.EquippedItem;
				equipManager.EquippedItem.Unequip();
			}
		}

        public override void WhileActive_State()
        {
            base.WhileActive_State();
            rigBody.velocity = new Vector2(direction * force, rigBody.velocity.y);
        }

        public override void OnExit_State()
        {
            base.OnExit_State();
            rigBody.velocity = new Vector2(0, rigBody.velocity.y);

			if (currEquipped != null)
			{
				currEquipped.Equip();
				currEquipped = null;
			}
		}

        private IEnumerator stateDuration()
        {
            yield return new WaitForSeconds(0.2f);
            controller.EndState(this);
            yield return new WaitForSeconds(ImmuneToDamageDuration);
            CanTakeDamage = true;
        }

        public void TakeDamage(int damage, float launchDirection = 0, float launchForce = 1, bool dmgSrcIsAttack = false, float immunity = -1)
        {
            if (controller.ActiveStateMovement != this && CanTakeDamage 
				&& !(controller.ActiveHighPriorityState is EnemyWakeUp || controller.ActiveStateMovement is EnemySleep || controller.ActiveStateMovement is EnemyGoToSleep)
				&& (!immuneToAttackDmg || !dmgSrcIsAttack))
            {
                CanTakeDamage = false;
                CharacterStats.TakeDamage(damage);
                direction = launchDirection;
				force = launchForce;
				ImmuneToDamageDuration = immunity < 0 ? initialImmunity : immunity;
                controller.SwapState(this);
            }
        }
    }
}
