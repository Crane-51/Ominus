using UnityEngine;
using DiContainerLibrary.DiContainer;
using Implementation.Data;
using General.State;
using System.Collections;

public class PlatformSwinging : HighPriorityState
{
	[SerializeField] private Transform pivotPoint;
	[SerializeField] private float swingingSpeed;
	[SerializeField] private float lowerLimit;
	[SerializeField] private float upperLimit;
	[SerializeField] private Transform leftBorder;
	[SerializeField] private Transform rightBorder;

	[SerializeField] private float force;
	[SerializeField] private float angleMax;
	[SerializeField] private float angleMin;
	[SerializeField] private float offsetInTime;
	private bool offsetDone = false;
	private bool wait = false;
	private Rigidbody2D rigBody;

	private bool playerOnPlatform = false;
	[InjectDiContainter]
	private IGameInformation gameInformation { get; set; }
	private bool changingDir = false;
	private LineRenderer lr;
	private Vector3[] positions = new Vector3[3];

	protected override void Initialization_State()
	{
		base.Initialization_State();
		lr = GetComponent<LineRenderer>();
		positions[0] = leftBorder.position;
		positions[1] = pivotPoint.position;
		positions[2] = rightBorder.position;
		lr.SetPositions(positions);
		rigBody = GetComponent<Rigidbody2D>();
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
		if (offsetDone && controller.ActiveHighPriorityState != this && !gameInformation.StopMovement)
		{
			controller.SwapState(this);
		}
	}

	public override void WhileActive_State()
	{
		base.WhileActive_State();
		//transform.RotateAround(pivotPoint.position, Vector3.forward, swingingSpeed * Time.deltaTime);
		lr.SetPosition(0, leftBorder.position);
		lr.SetPosition(2, rightBorder.position);
		//if (playerOnPlatform)
		//{
		//	gameInformation.Player.transform.rotation = Quaternion.identity;
		//}

		//float angle = transform.rotation.eulerAngles.z;
		//if (!((angle > 360 + lowerLimit && angle <= 360) || (angle >= 0 && angle < upperLimit) || (angle <= 0 && angle >= lowerLimit)))
		//{
		//	if (!changingDir)
		//	{
		//		swingingSpeed *= -1;
		//		changingDir = true;
		//	}
		//}
		//else
		//{
		//	changingDir = false;
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
			controller.EndState(this);
		}
	}

	//void OnCollisionEnter2D(Collision2D other)
	//{
	//	if (other.gameObject.tag == "Player")
	//	{
	//		other.collider.transform.SetParent(transform);
	//		playerOnPlatform = true;
	//	}
	//}

	//void OnCollisionExit2D(Collision2D other)
	//{
	//	if (other.gameObject.tag == "Player")
	//	{
	//		other.collider.transform.SetParent(null);
	//		playerOnPlatform = false;
	//	}

	//}
}
