using System.Collections;
using DiContainerLibrary.DiContainer;
using Enemy.State;
using General.State;
using Implementation.Data;
using Player.Movement;
using Player.Sneak;
using UnityEngine;

public class EnemyWakeUp : HighPriorityState
{
    /// <summary>
    /// Gets or sets physics overlap data.
    /// </summary>
    [InjectDiContainter]
    private IPhysicsOverlap physicsOverlap { get; set; }

    /// <summary>
    /// Gets or sets enemy data.
    /// </summary>
    [InjectDiContainter]
    private IGameInformation gameInformation { get; set; }

	[SerializeField] private float rangeOfWakeUp = 5f;

	private EnemyDetectTarget enemyDetectTarget;
	private EnemyInvestigateMovement enemyInvestigateMovement;
	private EnemyHealthBar healthBar;
	private EnemySharedDataAndInit sharedData;
	private SpriteRenderer sr;

	protected override void Initialization_State()
    {
        base.Initialization_State();
        Priority = -14;
        enemyDetectTarget = GetComponent<EnemyDetectTarget>();
		enemyInvestigateMovement = GetComponent<EnemyInvestigateMovement>();
		sharedData = GetComponent<EnemySharedDataAndInit>();
		sr = GetComponentInChildren<SpriteRenderer>();
		healthBar = sharedData.HealthBar;
    }

    public override void OnEnter_State()
    {
        base.OnEnter_State();
		sharedData.lastKnownPlayerPosition = gameInformation.Player.transform.position;
		sr.sortingOrder = 1000;
        StartCoroutine(WaitAnimationEnd());
    }

    public override void Update_State()
    {
        if (controller.ActiveStateMovement is EnemySleep
			&& !(controller.ActiveHighPriorityState is Character.Stats.CharacterIsDead)
			&& controller.ActiveHighPriorityState != this
			&& gameInformation.PlayerStateController.ToString() != ""
			&& 
			!(gameInformation.PlayerStateController.ActiveStateMovement is PlayerSneak
				|| gameInformation.PlayerStateController.ActiveStateMovement is PlayerSneakIdle
				|| gameInformation.PlayerStateController.ActiveStateMovement is PlayerIdle))
        {
			//if (enemyData.IsTargetStillInRangeOfVision(transform, gameInformation.Player.transform))
            if (physicsOverlap.Circle(transform, rangeOfWakeUp).Contains(gameInformation.Player))
            {
                controller.SwapState(this);
            }
        }
    }

	public override void WhileActive_State()
	{
		base.WhileActive_State();
		sharedData.lastKnownPlayerPosition = gameInformation.Player.transform.position;
	}

	public override void OnExit_State()
    {
        base.OnExit_State();
        SetStateOfOtherComponents(true);
		gameObject.layer = LayerMask.NameToLayer("Enemy");
		if (!(controller.ActiveStateMovement is EnemyInvestigateMovement) && !sharedData.targetLocked)
		{
			controller.ForceSwapState(enemyInvestigateMovement);
		}
	}

    private IEnumerator WaitAnimationEnd()
    {
        yield return new WaitUntil(() => designController.animationController.IsAnimationOver(this) );
        controller.EndState(this);
    }

    private void SetStateOfOtherComponents(bool isActive)
    {
        if (enemyDetectTarget != null)
        {
            enemyDetectTarget.enabled = isActive;
        }
		if (healthBar != null)
		{
			healthBar.gameObject.SetActive(isActive);
		}
	}
}
