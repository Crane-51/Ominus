using System.Collections;
using UnityEngine;

namespace Enemy.State
{
	public class EnemyFinalAttack : EnemyCombatState
	{
		protected override void Initialization_State()
		{
			base.Initialization_State();
			switch (controller.Id)
			{
				case "Monk":
				{
					clipLength = designController.animationController.GetAnimationClipLength("Kick");
					break;
				}
				case "SkeletonSwordman":
				{
					clipLength = designController.animationController.GetAnimationClipLength("SkeletonSlashRight");
					break;
				}
			}
		}

		public override void OnEnter_State()
		{
			base.OnEnter_State();
			sharedData.enemyData.CanAttack = false;
			StartCoroutine(AttackCooldown());
		}

		protected override IEnumerator AttackCooldown()
		{
			yield return new WaitForSeconds(clipLength);
			controller.EndState(this);
			yield return new WaitForSeconds(sharedData.enemyData.AttackCooldown);
			sharedData.enemyData.CanAttack = true;
		}
	}
}