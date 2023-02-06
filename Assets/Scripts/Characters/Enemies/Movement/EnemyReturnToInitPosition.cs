using General.State;
using UnityEngine;

namespace Enemy.State
{
	public class EnemyReturnToInitPosition : StateForMovement
	{
		private Vector3 initialPosition;
		private float initialDirection;
		[SerializeField] private float distanceTollerance = 1;
		private EnemySharedDataAndInit sharedData;

		protected override void Initialization_State()
		{
			base.Initialization_State();
			initialPosition = transform.position;
			initialDirection = transform.localScale.x;
			Priority = -14;
			sharedData = GetComponent<EnemySharedDataAndInit>();
		}

		public override void OnEnter_State()
		{
			base.OnEnter_State();
			sharedData.enemyData.LookAtTarget(transform, initialPosition);
			rigBody.velocity = new Vector2(0f, 0f);
		}

		public override void Update_State()
		{
			base.Update_State();

			if (controller.ActiveStateMovement != this && !(controller.ActiveHighPriorityState is Character.Stats.CharacterIsDead) &&
				!sharedData.targetLocked && (controller.ActiveStateMovement == null || controller.ActiveStateMovement is EnemyIdle) 
				&& controller.ActiveStateMechanic == null && Mathf.Abs(transform.position.x - initialPosition.x) > distanceTollerance)
			{
				controller.SwapState(this);
			}
		}

		public override void WhileActive_State()
		{
			base.WhileActive_State();
			if (Mathf.Abs(transform.position.x - initialPosition.x) > distanceTollerance)
			{
				rigBody.velocity = new Vector2(transform.localScale.x * 20 * Time.deltaTime * MovementData.MovementSpeed, rigBody.velocity.y); 
			}
			else
			{
				controller.EndState(this);
			}
		}

		public override void OnExit_State()
		{
			base.OnExit_State();
			if (controller.Id == "SkeletonArcher")
			{
				transform.localScale = new Vector3(initialDirection, transform.localScale.y);
			}
		}
	}
}