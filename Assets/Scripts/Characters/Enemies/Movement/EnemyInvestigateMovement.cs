using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;
using System.Collections;
using Character.Stats;

namespace Enemy.State
{
	public class EnemyInvestigateMovement : StateForMovement
	{
		private bool isLooking = false;
		private bool doneLooking = false;
		//private bool hitBorder = false;
		private EnemySharedDataAndInit sharedData;

		protected override void Initialization_State()
		{
			base.Initialization_State();
			Priority = 5;
			sharedData = GetComponent<EnemySharedDataAndInit>();
		}

		public override void OnEnter_State()
		{
			base.OnEnter_State();

			isLooking = false;
			doneLooking = false;
			//hitBorder = false;
			sharedData.enemyData.LookAtTarget(transform, sharedData.lastKnownPlayerPosition);
		}

		public override void WhileActive_State()
		{
			base.WhileActive_State();
			RaycastHit2D wallHit =
				Physics2D.Raycast(transform.position, transform.localScale.x == -1 ? Vector3.left : Vector3.right, 2f, LayerMask.GetMask("Environment", "Border"));
			//Debug.DrawRay(transform.position, (transform.localScale.x == -1 ? Vector3.left : Vector3.right) * 2f, Color.magenta);

			if (Mathf.Abs(transform.position.x - sharedData.lastKnownPlayerPosition.x) > 1 && !wallHit.collider)// && !hitBorder)
			{
				rigBody.velocity = new Vector2(MovementData.MovementSpeed * 20 * Time.deltaTime * sharedData.enemyData.LookAtTarget(transform, sharedData.lastKnownPlayerPosition), rigBody.velocity.y);
			}
			else if (!isLooking)
			{
				StartCoroutine(WaitForPlayer());
			}
		}

		public override void OnExit_State()
		{
			base.OnExit_State();
			designController.animationController.Anima.SetBool("EnemyDetectTarget", false);
		}

		private IEnumerator WaitForPlayer()
		{
			isLooking = true;
			rigBody.velocity = new Vector2(0, 0);
			designController.animationController.Anima.SetBool("EnemyInvestigateMovement", false);
			designController.animationController.Anima.SetBool("EnemyDetectTarget", true);
			//yield return new WaitForSeconds(3f);
			float timeout = 3f;
			while (!(controller.ActiveHighPriorityState is CharacterIsDead || controller.ActiveHighPriorityState is CharacterTakeDamage))
			{
				yield return null;
				timeout -= Time.deltaTime;
				if (timeout <= 0f)
					break;
			}
			doneLooking = true;
			designController.animationController.Anima.SetBool("EnemyDetectTarget", false);
			controller.EndState(this);
		}
	}
}