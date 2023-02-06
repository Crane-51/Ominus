using System.Collections;
using DiContainerLibrary.DiContainer;
using Enemy.State;
using General.State;
using Implementation.Data;
using Player.Sneak;
using UnityEngine;
using Player.Movement;

public class EnemyGoToSleep : StateForMovement
{
	/// <summary>
	/// Gets or sets enemy data.
	/// </summary>
	[InjectDiContainter]
	private IGameInformation gameInformation { get; set; }	
	private EnemyDetectTarget enemyDetectTarget { get; set; }
	private EnemySleep enemySleep { get; set; }
	private Vector3 initialPosition { get; set; }
	private EnemySharedDataAndInit sharedData;

	protected override void Initialization_State()
	{
		base.Initialization_State();
		Priority = -21;//TODO: check
		sharedData = GetComponent<EnemySharedDataAndInit>();
		enemyDetectTarget = GetComponent<EnemyDetectTarget>();
		enemySleep = GetComponent<EnemySleep>();
		SetStateOfOtherComponents(false);
		initialPosition = transform.position;
	}

	public override void OnEnter_State()
	{
		base.OnEnter_State();
		SetStateOfOtherComponents(false);
		StartCoroutine(WaitAnimationEnd());
		rigBody.velocity = new Vector2(0, 0);
		gameObject.layer = LayerMask.NameToLayer("IgnoreCharacters");
	}

	public override void Update_State()
	{
		if ((controller.ActiveHighPriorityState == null || !(controller.ActiveHighPriorityState is Character.Stats.CharacterIsDead)) &&
			!sharedData.targetLocked && 
			//(!sharedData.enemyData.IsTargetStillInRangeOfVision(transform, gameInformation.Player.transform)
			(!sharedData.targetInRangeOfVision
			|| gameInformation.PlayerStateController.ActiveStateMovement is PlayerIdle
			|| gameInformation.PlayerStateController.ActiveStateMovement is PlayerSneak
			|| gameInformation.PlayerStateController.ActiveStateMovement is PlayerSneakIdle)
			&& Mathf.Abs(transform.position.x - initialPosition.x) <= 1)
		{
			controller.SwapState(this);
		}
	}

	private IEnumerator WaitAnimationEnd()
	{
		yield return new WaitUntil(() => designController.animationController.IsAnimationOver(this));
		controller.SwapState(enemySleep);
		controller.EndState(this);
	}

	private void SetStateOfOtherComponents(bool isActive)
	{
		if (enemyDetectTarget != null)
		{
			enemyDetectTarget.enabled = isActive;
		}
	}
}
