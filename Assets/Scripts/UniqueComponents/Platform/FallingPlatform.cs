using System.Collections;
using System.Linq;
using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;

public class FallingPlatform : StateForMovement
{
	/// <summary>
	/// Gets or sets enemy data.
	/// </summary>
	[InjectDiContainter]
	private IGameInformation gameInformation { get; set; }
	[SerializeField] private float fallingSpeed;
	private bool waitingBeforeFalling = false;
	private bool canRise = false;
	[SerializeField] float fallingTimer = 2f;
	[SerializeField] float risingTimer = 4f;
	private Vector3 startingPosition;
	private bool isMoving = false;

	protected override void Initialization_State()
	{
		base.Initialization_State();
		startingPosition = transform.position;
	}

	public override void Update_State()
	{
		base.Update_State();
		if (isMoving && controller.ActiveStateMovement != this && !gameInformation.StopMovement)
		{
			controller.SwapState(this);
		}
	}

	public override void OnEnter_State()
	{
		base.OnEnter_State();
		isMoving = true;
		rigBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
	}

	public override void WhileActive_State()
	{
		base.WhileActive_State();
		rigBody.velocity = new Vector2(0f, fallingSpeed); 	

		if (canRise && Mathf.Abs(transform.position.y - startingPosition.y) < 0.3f)
		{
			isMoving = false;
			controller.EndState(this);
		}
	}

	public override void OnExit_State()
	{
		base.OnExit_State();
		rigBody.constraints = RigidbodyConstraints2D.FreezeAll;

		if (!isMoving)
		{
			if (!canRise)
			{
				StartCoroutine(WaitThenRise());
			}
			else
			{
				canRise = false;
				fallingSpeed *= -2;
			} 
		}
	}

	private IEnumerator WaitThenFall()
	{
		waitingBeforeFalling = true;
		yield return new WaitForSeconds(fallingTimer);
		waitingBeforeFalling = false;
		controller.SwapState(this);
	}

	private IEnumerator WaitThenRise()
	{
		canRise = false;
		yield return new WaitForSeconds(risingTimer);
		canRise = true;
		if (controller.ActiveStateMovement != this)
		{
			fallingSpeed *= -1/2f;
			controller.SwapState(this); 
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player" && controller.ActiveStateMovement != this)
		{
			StartCoroutine(WaitThenFall());
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Ground")
		{
			isMoving = false;
			controller.EndState(this);
		}
	}
}
