using System.Collections;
using Character.Stats;
using General.Enums;
using General.State;
using UnityEngine;
using DiContainerLibrary.DiContainer;
using Implementation.Data;

public class SwingingMovement : StateForMovement
{
	//[SerializeField] private DirectionEnum direction;

	[SerializeField] private float force;
	[SerializeField] private float angleMax;
	[SerializeField] private float angleMin;
	[SerializeField] private int damage;
	[SerializeField] private float offsetInTime;
	//[SerializeField] private bool canDmg = true;
	//[SerializeField] private bool standable = false;

	private bool offsetDone;
	private bool angleMinReached;
	private bool angleMaxReached;
	private int dir;
	private bool playerOnPlatform = false;
	private Vector3 currPosition;
	private Vector3 prevPosition;
	bool wait = false;


	[InjectDiContainter]
	private IGameInformation gameInformation { get; set; }

	protected override void Initialization_State()
	{
		base.Initialization_State();
		//dir = (int)direction;
		offsetDone = false;
		Priority = 1;
		//offsetDone = true;
		StartCoroutine(StartSwinging());
	}

	private IEnumerator StartSwinging()
	{
		yield return new WaitForSeconds(offsetInTime);
		offsetDone = true;
	}

	public override void Update_State()
	{
		base.Update_State();
		if (offsetDone && controller.ActiveStateMovement != this && !gameInformation.StopMovement)
		{
			controller.SwapState(this);
		}
	}

	public override void OnEnter_State()
	{
		base.OnEnter_State();
		rigBody.bodyType = RigidbodyType2D.Dynamic;
	}

	public override void WhileActive_State()
	{
		base.WhileActive_State();

		//rigBody.AddTorque(dir * force * Time.deltaTime);
		////var eulerDegrees = transform.rotation.eulerAngles.z;
		//if (transform.rotation.z > angleMax && !angleMaxReached)
		//{
		//	rigBody.velocity = Vector3.zero;
		//	dir = -1;
		//	angleMaxReached = true;
		//	angleMinReached = false;
		//	//designController.soundController.PlaySound(this);
		//}
		//else if (transform.rotation.z < angleMin && !angleMinReached)
		//{
		//	rigBody.velocity = Vector3.zero;
		//	dir = 1;
		//	angleMaxReached = false;
		//	angleMinReached = true;
		//	//designController.soundController.PlaySound(this);
		//}

		if (transform.rotation.z >= 0 && transform.rotation.z < angleMax
			&& rigBody.angularVelocity >= 0 && rigBody.angularVelocity < force)
		{
			rigBody.angularVelocity = force;
		}
		else if (transform.rotation.z < 0 && transform.rotation.z > angleMin
			&& rigBody.angularVelocity < 0 && rigBody.angularVelocity > force * -1)
		{
			rigBody.angularVelocity = force * -1;
		}

		if (gameInformation.StopMovement)
		{
			rigBody.bodyType = RigidbodyType2D.Kinematic;
			controller.EndState(this);
		}

		//var hinge = GetComponent<HingeJoint2D>();
		//if ((hinge.limitState == JointLimitState2D.UpperLimit || hinge.limitState == JointLimitState2D.LowerLimit) && !wait)
		//{
		//	wait = true;
		//	JointMotor2D motor = hinge.motor;
		//	motor.motorSpeed *= -1;
		//	hinge.motor = motor;
		//}
		//else if (hinge.limitState == JointLimitState2D.Inactive && wait)
		//{
		//	wait = false;
		//}

		//if (playerOnPlatform)
		//{
		//	Vector3 diff = currPosition - prevPosition;
		//	gameInformation.Player.GetComponent<Rigidbody2D>().MovePosition(gameInformation.Player.transform.position + diff);
		//}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var playerTakeDmg = collision.GetComponent<CharacterTakeDamage>();

		if (playerTakeDmg != null)
		{
			playerTakeDmg.TakeDamage(damage, dir);
		}
	}

	//void OnCollisionEnter2D(Collision2D other)
	//{
	//	if (standable)
	//	{
	//		if (gameInformation == null) return;
	//		if (other.gameObject == gameInformation.Player)
	//		{
	//			other.collider.transform.SetParent(transform);
	//			prevPosition = transform.position;
	//			gameInformation.Player.GetComponent<Rigidbody2D>().isKinematic = true;
	//			playerOnPlatform = true;
	//		}
	//	}
	//}

	void OnCollisionExit2D(Collision2D other)
	{
		//if (standable)
		//{
		//	if (other.gameObject == gameInformation.Player)
		//	{
		//		//other.collider.transform.SetParent(null);
		//		playerOnPlatform = false;
		//		gameInformation.Player.GetComponent<Rigidbody2D>().isKinematic = false;
		//	}
		//}
	}
}
