using System.Linq;
using Character.Stats;
using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;
using General.Enums;

public class RollingStone : StateForMovement
{
	[InjectDiContainter]
	protected IPhysicsOverlap overlap { get; set; }

	/// <summary>
	/// The damage of rolling stone.
	/// </summary>
	[SerializeField] protected int damage;

	/// <summary>
	/// Speed of rolling
	/// </summary>
	[SerializeField] protected float rollingForce;

	/// <summary>
	/// Max allowed force.
	/// </summary>
	[SerializeField] protected float maxAllowedForce;

	/// <summary>
	/// Movement speed.
	/// </summary>
	[SerializeField] protected float movementSpeed;
	private PoolObjectKey key;

	[InjectDiContainter]
	protected IGameInformation gameInformation { get; set; }
	[SerializeField] protected bool activateByTrigger = false;

	protected override void Initialization_State()
	{
		if (activateByTrigger)
		{
			base.Initialization_State();
			//controller.SwapState(this);

		}
	}

	private void OnEnable()
	{
		if (!activateByTrigger)
		{
			base.Initialization_State();
			controller.SwapState(this);
		}
	}

	public override void Update_State()
	{
		base.Update_State();
		if (controller.ActiveStateMovement != this && !gameInformation.StopMovement && !activateByTrigger)
		{
			controller.SwapState(this);
		}
	}

	public override void WhileActive_State()
    {
        var targetHit = overlap.Circle(transform, transform.localScale.x/12).FirstOrDefault(x => x.GetComponent<CharacterStatsMono>() != null);

        if (targetHit != null)
        {
            var takeDamage = targetHit.GetComponent<CharacterTakeDamage>();
            takeDamage.TakeDamage(damage);
        }

        if (rigBody.velocity.x < maxAllowedForce)
        {
            rigBody.AddTorque(-rollingForce);
            rigBody.velocity = new Vector2(movementSpeed, rigBody.velocity.y);
        }

		if (gameInformation.StopMovement)
		{
			controller.EndState(this);
		}
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.gameObject.tag == "Border")
		{
			//Destroy(this.gameObject);
			ObjectPooler.pooler.PushObject(gameObject, key);
		}
	}

	public void StartState(int damage, float rollingForce, float maxAllowedForce, float movementSpeed, PoolObjectKey key)
    {
        this.damage = damage;
        this.rollingForce = rollingForce;
        this.maxAllowedForce = maxAllowedForce;
        this.movementSpeed = movementSpeed;
		this.key = key;
    }

    public void ChangeDirection(float rollingForce, float maxAllowedForce, float movementSpeed)
    {
        this.rollingForce = rollingForce;
        this.maxAllowedForce = maxAllowedForce;
        this.movementSpeed = movementSpeed;
    }

	public void Activate()
	{
		controller.SwapState(this);
	}
}
