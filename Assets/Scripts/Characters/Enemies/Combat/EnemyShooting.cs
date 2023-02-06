using System.Collections;
using UnityEngine;
using General.State;
using Implementation.Data;
using DiContainerLibrary.DiContainer;
using Character.Stats;
using General.Enums;


namespace Enemy.State
{
	public class EnemyShooting : StateForMechanics
	{
		/// <summary>
		/// Gets or sets enemy data.
		/// </summary>
		[InjectDiContainter]
		private IGameInformation gameInformation { get; set; }
		private EnemyInvestigateMovement enemyInvestigateMovement;
		private EnemySharedDataAndInit sharedData;
		private float clipLength = 0f;
		private DmgAndDestroyOnCollision ddc;

		[SerializeField] private Transform bulletSpawn;
		[SerializeField] private PoolObjectKey bullet;
		[SerializeField] private float bulletSpeed = 7f;

		protected override void Initialization_State()
		{
			base.Initialization_State();
			Priority = 16;
			clipLength = designController.animationController.GetAnimationClipLength("Aiming");
			enemyInvestigateMovement = GetComponent<EnemyInvestigateMovement>();
			sharedData = GetComponent<EnemySharedDataAndInit>();
		}

		public override void OnEnter_State()
		{
			base.OnEnter_State();
			GameObject arrow = ObjectPooler.pooler.PullObject(bullet);

			//Vector3 rotation = bulletSpawn.transform.rotation.eulerAngles;
			//rotation.x = 0f;
			//rotation.y = 0f;
			//arrow.transform.rotation = Quaternion.Euler(rotation);
			//arrow.transform.localScale = transform.localScale;
			if (arrow != null)
			{
				arrow.transform.position = bulletSpawn.position;
				arrow.transform.rotation = bulletSpawn.rotation;
				arrow.SetActive(true);
				ddc = arrow.GetComponent<DmgAndDestroyOnCollision>();
				ddc.dmg = DiceRoller.RollDieWithModifier(sharedData.weaponData.DieToRoll,
					sharedData.weaponData.Type == WeaponType.Melee ? sharedData.enemyStats.Strength : sharedData.enemyStats.Dexterity);
				ddc.dmgTag = "Player";
				//arrow.GetComponent<DestroyOnCollision>().dmg = 0;
				//Vector2 direction = new Vector2(transform.localScale.x, 0f);
				Vector2 direction = bulletSpawn.right;
				//direction.x *= bulletSpawn.lossyScale.x;
				arrow.GetComponent<Rigidbody2D>().AddForce(direction * bulletSpeed, ForceMode2D.Impulse);
				sharedData.enemyData.CanAttack = false;
				StartCoroutine(AttackCooldown()); 
			}
		}

		public override void OnExit_State()
		{
			base.OnExit_State();
			if (!sharedData.targetLocked)
			{
				controller.ForceSwapState(enemyInvestigateMovement);
			}
		}

		private IEnumerator AttackCooldown()
		{
			yield return new WaitForSeconds(clipLength);
			controller.EndState(this);
			yield return new WaitForSeconds(sharedData.enemyData.AttackCooldown);
			sharedData.enemyData.CanAttack = true;
		}
	}
}