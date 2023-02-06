using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;

public class CrosshairMovement : StateForMovement
{
	[InjectDiContainter]
	protected IGameInformation gameInformation { get; set; }
	private SpriteRenderer sr;
	public float MovementDirection { get; private set; }
	private float lastMovementDirection;
	[SerializeField] private float movementSpeed = 1f;
	public float MaxAngle
	{
		get
		{
			return maxAngle;
		}
		private set
		{
			maxAngle = value;
		}
	}
	[SerializeField] private float maxAngle = 45f;
	public float Angle { get; private set; }
	public int UpDownIndicator { get; private set; }

	protected override void Initialization_State()
	{
		base.Initialization_State();
		sr = GetComponent<SpriteRenderer>();
		MovementDirection = 0f;
		UpDownIndicator = 0;
	}

	public override void Update_State()
	{
		base.Update_State();
		if (sr.enabled && !gameInformation.StopMovement)
		{
			MovementDirection = (Input.GetKey(KeyCode.UpArrow) ? 1 : 0) + (Input.GetKey(KeyCode.DownArrow) ? -1 : 0);
			if (MovementDirection != 0)
			{
				Angle = Vector3.Angle(Vector2.right * transform.parent.localScale.x, transform.position - transform.parent.position);
				if ((Angle >= 0 && Angle <= maxAngle) || MovementDirection != lastMovementDirection)
				{
					transform.RotateAround(transform.parent.position, Vector3.forward, MovementDirection * movementSpeed * transform.parent.localScale.x);
					lastMovementDirection = MovementDirection;
				}

				var diff = transform.position.y - transform.parent.position.y;
				if (diff > 0)
				{
					UpDownIndicator = 1;
				}
				else if (diff < 0)
				{
					UpDownIndicator = -1;
				}
				else
				{
					UpDownIndicator = 0;
				}
			}
		}
	}

	// Start is called before the first frame update
	//void Start()
	//   {
	//	sr = GetComponent<SpriteRenderer>();
	//	MovementDirection = 0f;
	//	UpDownIndicator = 0;
	//}

	// Update is called once per frame
	//void Update()
 //   {
	//	if (sr.enabled)
	//	{
	//		MovementDirection = (Input.GetKey(KeyCode.UpArrow) ? 1 : 0) + (Input.GetKey(KeyCode.DownArrow) ? -1 : 0);
	//		if (MovementDirection != 0)
	//		{
	//			Angle = Vector3.Angle(Vector2.right * transform.parent.localScale.x, transform.position - transform.parent.position);
	//			if ((Angle >= 0 && Angle <= maxAngle) || MovementDirection != lastMovementDirection)
	//			{
	//				transform.RotateAround(transform.parent.position, Vector3.forward, MovementDirection * movementSpeed * transform.parent.localScale.x);
	//				lastMovementDirection = MovementDirection;
	//			}

	//			var diff = transform.position.y - transform.parent.position.y;
	//			if (diff > 0)
	//			{
	//				UpDownIndicator = 1;
	//			}
	//			else if (diff < 0)
	//			{
	//				UpDownIndicator = -1;
	//			}
	//			else
	//			{
	//				UpDownIndicator = 0;
	//			}
	//		}
	//	}
 //   }

	public void EnableCrosshair(bool enable)
	{
		sr.enabled = enable;		
	}

	public bool IsEnabled()
	{
		return sr.enabled;
	}
}
