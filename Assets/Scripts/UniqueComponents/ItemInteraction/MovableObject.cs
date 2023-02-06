using UnityEngine;
using System.Linq;
using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;

public class MovableObject : StateForMovement
{
	private bool isGrabbed = false;
	[InjectDiContainter]
	private IPhysicsOverlap physicsOverlap { get; set; }
	private SpriteRenderer sr;
	private float height;
	private float width;
	private Vector2 offset;
	private float distance = 0.5f;
	private float distanceToUse;
	private bool playerLeft = false;
	private bool releasedAndGrounded = true;
	[SerializeField] private float fallingSpeed = 1f;

	protected override void Initialization_State()
	{
		base.Initialization_State();
		sr = GetComponentInChildren<SpriteRenderer>();
		height = sr.bounds.size.y;
		width = sr.bounds.size.x;
		offset = new Vector2(width / 2f + distance / 2f, -height / 2f /*- distance / 2f*/);
	}

	public override void Update_State()
	{
		base.Update_State();
		if (controller.ActiveStateMovement != this && !releasedAndGrounded)
		{
			controller.SwapState(this);
		}
	}

	public override void OnEnter_State()
	{
		base.OnEnter_State();
		if (releasedAndGrounded)
		{
			isGrabbed = true;
			rigBody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
			rigBody.isKinematic = true;
			releasedAndGrounded = false; 
		}
	}

	public override void WhileActive_State()
	{
		base.WhileActive_State();
		if (isGrabbed)
		{
			if (rigBody.velocity.magnitude != 0)
			{
				if (rigBody.velocity.x > 0)
				{
					distanceToUse = playerLeft ? distance / 4f : distance;
					if (physicsOverlap.Box(new Vector2(transform.position.x + offset.x, transform.position.y), distanceToUse, height).Any(x => x.gameObject.tag == "Ground"))
					{
						rigBody.constraints = RigidbodyConstraints2D.FreezeAll;
					}
					else
					{
						rigBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
					}
				}
				else if (rigBody.velocity.x < 0)
				{
					distanceToUse = playerLeft ? distance : distance / 4f;
					if (physicsOverlap.Box(new Vector2(transform.position.x - offset.x, transform.position.y), distanceToUse, height).Any(x => x.gameObject.tag == "Ground"))
					{
						rigBody.constraints = RigidbodyConstraints2D.FreezeAll;
					}
					else
					{
						rigBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
					}
				}

				if (!physicsOverlap.Box(new Vector2(transform.position.x, transform.position.y + offset.y), width, distance).Any(x => x.gameObject.tag == "Ground" && x.gameObject != this.gameObject))
				{
					rigBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
				}
			}
		}
		else if (!releasedAndGrounded)
		{
			if (physicsOverlap.Box(new Vector2(transform.position.x, transform.position.y + offset.y), width, 0.25f).Any(x => x.gameObject.tag == "Ground" && x.gameObject != this.gameObject))
			{
				releasedAndGrounded = true;
				controller.EndState(this);
			}
			else
			{
				rigBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
				rigBody.velocity = new Vector3(rigBody.velocity.x, fallingSpeed, 0f);
			}
		}
	}

	public override void OnExit_State()
	{
		base.OnExit_State();
		rigBody.constraints = RigidbodyConstraints2D.FreezeAll;
		
		rigBody.isKinematic = false;
	}

	//private void FixedUpdate()
	//{
	//	//physicsOverlap.Box(new Vector2(transform.position.x, transform.position.y), width, height);
	//	//physicsOverlap.Box(new Vector2(transform.position.x + offset.x, transform.position.y), distance, height);
	//	//physicsOverlap.Box(new Vector2(transform.position.x, transform.position.y + offset.y), width, distance);

	//	if (isGrabbed)
	//	{
	//		if (rigBody.velocity.magnitude != 0)
	//		{
	//			if (rigBody.velocity.x > 0)
	//			{
	//				distanceToUse = playerLeft ? distance / 4f : distance;
	//				if (physicsOverlap.Box(new Vector2(transform.position.x + offset.x, transform.position.y), distanceToUse, height).Any(x => x.gameObject.tag == "Ground"))
	//				{
	//					rigBody.constraints = RigidbodyConstraints2D.FreezeAll;
	//				}
	//				else
	//				{
	//					rigBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
	//				}
	//			}
	//			else if (rigBody.velocity.x < 0)
	//			{
	//				distanceToUse = playerLeft ? distance : distance / 4f;
	//				if (physicsOverlap.Box(new Vector2(transform.position.x - offset.x, transform.position.y), distanceToUse, height).Any(x => x.gameObject.tag == "Ground"))
	//				{
	//					rigBody.constraints = RigidbodyConstraints2D.FreezeAll;
	//				}
	//				else
	//				{
	//					rigBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
	//				}
	//			}

	//			if (!physicsOverlap.Box(new Vector2(transform.position.x, transform.position.y + offset.y), width, distance).Any(x => x.gameObject.tag == "Ground"))
	//			{
	//				rigBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
	//			}
	//		}
	//	}
	//	else if (!releasedAndGrounded)
	//	{
	//		if (physicsOverlap.Box(new Vector2(transform.position.x, transform.position.y + offset.y), width, 0.1f).Any(x => x.gameObject.tag == "Ground"))
	//		{
	//			rigBody.constraints = RigidbodyConstraints2D.FreezeAll;
	//			releasedAndGrounded = true;
	//			rigBody.isKinematic = false;
	//		}
	//		else
	//		{				
	//			rigBody.velocity = new Vector3(rigBody.velocity.x, fallingSpeed, 0f);
	//		}
	//	}
	//}

	public void GrabObject(Vector3 playerPosition)
	{
		if (transform.position.x - playerPosition.x > 0)
		{
			playerLeft = true;
		}
		else
		{
			playerLeft = false;
		}

		if (controller.ActiveStateMovement != this)
		{
			controller.SwapState(this); 
		}
	}

	public void ReleaseObject()
	{
		isGrabbed = false;
		
		if (physicsOverlap.Box(new Vector2(transform.position.x, transform.position.y + offset.y), width, distance).Any(x => x.gameObject.tag == "Ground"))
		{
			releasedAndGrounded = true;
			controller.EndState(this);
		}
		else
		{
			rigBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
		}
	}
}
