using System.Collections;
using UnityEngine;

namespace Enemy.State
{
	public class EnemyComboAttack : EnemyCombatState
	{
		protected override void Initialization_State()
		{
			base.Initialization_State();
			switch (controller.Id)
			{
				case "Monk":
				{
					clipLength = designController.animationController.GetAnimationClipLength("RightStrike");
					break;
				}
				case "SkeletonSwordman":
				{
					clipLength = designController.animationController.GetAnimationClipLength("SkeletonSlashLeft");
					break;
				}
			}
		}

		public override void OnEnter_State()
		{
			base.OnEnter_State();
			StartCoroutine(AttackCooldown());
		}
	}
}