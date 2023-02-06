using System.Collections;
using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;
using Character.Stats;
using System.Linq;

namespace Enemy.State
{
	public class EnemyCombatState : StateForMechanics
	{
		[InjectDiContainter]
		protected IPhysicsOverlap physicsOverlap { get; set; }
		[InjectDiContainter]
		protected IGameInformation gameInformation { get; set; }
		protected float clipLength = 0f;
		protected EnemySharedDataAndInit sharedData;

		private CharacterTakeDamage target;
		[SerializeField] private float impactForce = 1f;
		

		protected override void Initialization_State()
		{
			base.Initialization_State();
			Priority = 16;
			sharedData = GetComponent<EnemySharedDataAndInit>();
			target = gameInformation.Player.GetComponent<CharacterTakeDamage>();
		}

		public override void OnEnter_State()
		{
			base.OnEnter_State();
			var target = physicsOverlap.Box(transform, sharedData.enemyData.MinRangeOfAttack).Where(x => x.tag == "Player" && x.GetComponent<CharacterTakeDamage>() != null 
				&& !(x.GetComponent<StateController>().ActiveHighPriorityState is CharacterIsDead)).Select(x => x.GetComponent<CharacterTakeDamage>()).ToList().FirstOrDefault();

			if (target != null)
			{
				int dmg = DiceRoller.RollDieWithModifier(sharedData.weaponData.DieToRoll, 
					sharedData.weaponData.Type == General.Enums.WeaponType.Melee ? sharedData.enemyStats.Strength : sharedData.enemyStats.Dexterity);
				target.TakeDamage(dmg, transform.localScale.x, impactForce);
			}
		}
		
		protected virtual IEnumerator AttackCooldown()
		{
			yield return new WaitForSeconds(clipLength);
			controller.EndState(this);
		}
	}
}
