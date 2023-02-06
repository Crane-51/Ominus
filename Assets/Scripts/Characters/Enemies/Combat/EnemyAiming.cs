using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General.State;
using Implementation.Data;
using DiContainerLibrary.DiContainer;
using Character.Stats;

namespace Enemy.State
{
	public class EnemyAiming : StateForMechanics
	{
		/// <summary>
		/// Gets or sets enemy data.
		/// </summary>
		[InjectDiContainter]
		private IGameInformation gameInformation { get; set; }

		/// <summary>
		/// Gets or sets player stats.
		/// </summary>
		private CharacterTakeDamage target { get; set; }
		private EnemyShooting shootingState;
		private bool aiming = false;
		private EnemySharedDataAndInit sharedData;

		[SerializeField] private GameObject bulletSpawn;
		[SerializeField] private SpriteRenderer frontSide;
		[SerializeField] private GameObject skeletonBody;
		[SerializeField] private GameObject skeletonBackArm;
		[SerializeField] private GameObject skeletonFrontArm;

		protected override void Initialization_State()
		{
			base.Initialization_State();
			Priority = 15;
			target = gameInformation.Player.GetComponent<CharacterTakeDamage>();
			shootingState = GetComponent<EnemyShooting>();
			sharedData = GetComponent<EnemySharedDataAndInit>();
		}

		public override void OnEnter_State()
		{
			base.OnEnter_State();
			sharedData.enemyData.LookAtTarget(transform, target.transform);
		}


		public override void Update_State()
		{
			base.Update_State();
			if (controller.ActiveStateMechanic != this && sharedData.enemyData.CanAttack && !(controller.ActiveHighPriorityState is CharacterIsDead))
			{
				if (sharedData.targetLocked && (sharedData.enemyData.IsTargetInRangeOfRangedAttack(transform, gameInformation.Player.transform) || sharedData.forceAim))
				{
					controller.SwapState(this);
				}
			}
		}

		public override void WhileActive_State()
		{
			//GetComponent<EnemyDetectTarget>().lastKnownPlayerPosition = gameInformation.Player.transform.position;
			if (designController.animationController.IsAnimationOver(this) && !aiming)
			{
				Vector2 direction = (gameInformation.Player.transform.position - bulletSpawn.transform.position).normalized;
				Quaternion rotation = Quaternion.FromToRotation(new Vector2(transform.localScale.x, 0f), direction);

				if (Vector2.Distance(gameInformation.Player.transform.position, bulletSpawn.transform.position) > 1.5f)
				{
					frontSide.enabled = false;
					skeletonBackArm.transform.rotation = rotation;
					skeletonFrontArm.transform.rotation = rotation;
					skeletonBody.GetComponent<SpriteRenderer>().enabled = true;
					skeletonBackArm.GetComponent<SpriteRenderer>().enabled = true;
					skeletonFrontArm.GetComponent<SpriteRenderer>().enabled = true; 
				}
				if (transform.localScale.x == -1)
				{
					Vector3 rot = rotation.eulerAngles;
					rot = new Vector3(rot.x, rot.y, rot.z + 180);
					rotation = Quaternion.Euler(rot);
				}
				bulletSpawn.transform.rotation = rotation;
				aiming = true;
				StartCoroutine(WaitAndAim());

			}
		}

		public override void OnExit_State()
		{
			base.OnExit_State();
			aiming = false;
			frontSide.enabled = true;
			skeletonBody.GetComponent<SpriteRenderer>().enabled = false;
			skeletonBackArm.GetComponent<SpriteRenderer>().enabled = false;
			skeletonFrontArm.GetComponent<SpriteRenderer>().enabled = false;
		}

		private IEnumerator WaitAndAim()
		{
			yield return new WaitForSeconds(0.25f);
			controller.SwapState(shootingState);
			controller.EndState(this);

		}

	}
}