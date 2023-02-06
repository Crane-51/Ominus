using System.Collections;
using Character.Stats;
using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;

public class CrushingWall : HighPriorityState {

    [InjectDiContainter]
    private IGameInformation gameInformation { get; set; }
    [SerializeField] private int damage;
	[SerializeField] private float movementSpeed;
	[SerializeField] private float directionTimeOffset;

	private float realMovementSpeed;
	private float direction;
	private bool waitForTimer = false;

    protected override void Initialization_State()
    {
        base.Initialization_State();
        direction = -1;
        controller.SwapState(this);
    }

	public override void Update_State()
	{
		base.Update_State();
		if (controller.ActiveHighPriorityState != this && !gameInformation.StopMovement && !waitForTimer)
		{
			controller.SwapState(this);
		}
	}

	public override void OnEnter_State()
    {
        base.OnEnter_State();
        if (direction < 0)
        {
            realMovementSpeed = movementSpeed;
        }
        else
        {
            realMovementSpeed = movementSpeed /3;
        }
    }

    public override void WhileActive_State()
    {
        base.WhileActive_State();
        transform.Translate(new Vector2(0, direction * realMovementSpeed * Time.deltaTime));
		if (gameInformation.StopMovement)
		{
			controller.EndState(this);
		}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
		//Debug.Log(collision.gameObject.tag);
		//if(controller.ActiveHighPriorityState == this && collision.gameObject.tag =="Ground")
		//{
		//    controller.EndState(this);
		//    StartCoroutine(WaitTimer());           
		//}

		//Debug.Log(collision.gameObject.tag);
		if (controller.ActiveHighPriorityState == this && (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player"))
		{
			controller.EndState(this);
			StartCoroutine(WaitTimer());
		}

		//else if (collision.gameObject == gameInformation.Player)
		//{
		//	var takeDamage = collision.gameObject.GetComponent<CharacterTakeDamage>();
		//	if (takeDamage != null)
		//	{
		//		takeDamage.TakeDamage(damage, -1);
		//	}
		//	controller.EndState(this);
		//	StartCoroutine(WaitTimer());
		//}
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
		//Debug.Log(collision.gameObject.tag);
		//if (controller.ActiveHighPriorityState == this && (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player"))
		//{
		//	controller.EndState(this);
		//	StartCoroutine(WaitTimer());
		//}

		//else
		if (collision.gameObject == gameInformation.Player)
		{
			var takeDamage = collision.gameObject.GetComponent<CharacterTakeDamage>();
			if (takeDamage != null)
			{
				takeDamage.TakeDamage(damage, -1);
			}
			controller.EndState(this);
			StartCoroutine(WaitTimer());
		}
	}
    private IEnumerator WaitTimer()
    {
		waitForTimer = true;
        yield return new WaitForSeconds(directionTimeOffset);
		waitForTimer = false;
        direction = -direction;
        controller.SwapState(this);
    }
}
