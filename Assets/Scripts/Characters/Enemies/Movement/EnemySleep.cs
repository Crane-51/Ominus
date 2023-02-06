using Enemy.State;
using General.State;
using UnityEngine;

public class EnemySleep : StateForMovement
{
	private EnemyDetectTarget enemyDetectTarget { get; set; }
	private EnemyHealthBar healthBar;
	private SpriteRenderer sr;

	protected override void Initialization_State()
	{
		base.Initialization_State();
		Priority = -20;//TODO: check
		enemyDetectTarget = GetComponent<EnemyDetectTarget>();
		healthBar = GetComponent<EnemySharedDataAndInit>().HealthBar;
		SetStateOfOtherComponents(false);
		sr = GetComponentInChildren<SpriteRenderer>();
		controller.SwapState(this);
	}

	public override void OnEnter_State()
	{
		rigBody.velocity = new Vector2(0, 0);
		base.OnEnter_State();
		SetStateOfOtherComponents(false);
		sr.sortingOrder = 40;
		gameObject.layer = LayerMask.NameToLayer("IgnoreCharacters");
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
