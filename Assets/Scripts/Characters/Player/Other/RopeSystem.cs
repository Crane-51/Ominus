using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using General.Enums;

public class RopeSystem : MonoBehaviour
{
	//public GameObject ropeHingeAnchor;
	private DistanceJoint2D ropeJoint;
	//public Transform crosshair;
	//public CrosshairMovement crosshairMovement;
	public bool RopeAttached { get; private set; }
	//private Vector2 playerPosition;
	//private Rigidbody2D ropeHingeAnchorRb;
	//private SpriteRenderer ropeHingeAnchorSprite;
	private LineRenderer ropeRenderer;
	//public LayerMask ropeLayerMask;
	//private float ropeMaxCastDistance = 20f;
	//private List<Vector2> ropePositions = new List<Vector2>();
	//private bool distanceSet;
	public Transform Origin { get; private set; }
	public Transform Hook { get; private set; }
	private Vector3 position;
	[SerializeField] private EquipmentManager equipManager;
	public Vector3 RopeDir { get; private set; }
	public AnchorType Anchor { get; private set; }
	[SerializeField] private float ropeMaxLength = 7f;

	void Awake()
	{
		//ropeJoint.enabled = false;
		//playerPosition = transform.position;
		//ropeHingeAnchorRb = ropeHingeAnchor.GetComponent<Rigidbody2D>();
		//ropeHingeAnchorSprite = ropeHingeAnchor.GetComponent<SpriteRenderer>();
		ropeRenderer = GetComponent<LineRenderer>();
		ropeJoint = GetComponentInParent<DistanceJoint2D>();
		RopeAttached = false;
	}

	//void Update()
	//{
	//	//var worldMousePosition =
	//	//	Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
	//	//var facingDirection = worldMousePosition - transform.position;
	//	//var aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
	//	//if (aimAngle < 0f)
	//	//{
	//	//	aimAngle = Mathf.PI * 2 + aimAngle;
	//	//}

	//	//var aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;
	//	playerPosition = transform.position;

	//	if (!ropeAttached)
	//	{
	//		crosshairMovement.EnableCrosshair(true);
	//	}
	//	else
	//	{
	//		crosshairMovement.EnableCrosshair(false);
	//	}
	//	//HandleInput(aimDirection);
	//	HandleInput();
	//	UpdateRopePositions();
	//}

	private void FixedUpdate()
	{
		if (ropeRenderer.enabled)
		{
			if (Hook != null && Hook.gameObject.activeInHierarchy)
			{
				RopeDir = (Origin.position - Hook.position).normalized;
				position = Origin.position;
				position.z = -1;
				ropeRenderer.SetPosition(0, position);
				position = Hook.position;
				position.z = -1;
				ropeRenderer.SetPosition(1, position);
				RopeAttached = Hook.GetComponent<HookBehaviour>().RopeAttached;
				if (RopeAttached)
				{
					equipManager.UnequippedRanged();
					Anchor = Hook.GetComponent<HookBehaviour>().Anchor;
					//Debug.Log(Anchor);
				}
				else if (Vector2.Distance(Origin.position, Hook.position) > ropeMaxLength)
				{
					ResetRope();
					equipManager.EquippedRanged();
				}
			}
			else
			{
				//ropeRenderer.enabled = false;
				ResetRope();
				equipManager.EquippedRanged();
			}
		}
	}

	//private void HandleInput(Vector2 aimDirection)
	//private void HandleInput()
	//{
	//	if (Input.GetKeyDown(KeyCode.D))
	//	{
	//		if (ropeAttached)
	//		{
	//			ResetRope();
	//			return;
	//		}
			
	//		ropeRenderer.enabled = true;

	//		var hit = Physics2D.Raycast(playerPosition, aimDirection, ropeMaxCastDistance, ropeLayerMask);

	//		if (hit.collider != null)
	//		{
	//			ropeAttached = true;
	//			if (!ropePositions.Contains(hit.point))
	//			{
	//				// Jump slightly to distance the player a little from the ground after grappling to something.
	//				transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 2f), ForceMode2D.Impulse);
	//				ropePositions.Add(hit.point);
	//				ropeJoint.distance = Vector2.Distance(playerPosition, hit.point);
	//				ropeJoint.enabled = true;
	//				ropeHingeAnchorSprite.enabled = true;
	//			}
	//		}
	//		else
	//		{
	//			ropeRenderer.enabled = false;
	//			ropeAttached = false;
	//			ropeJoint.enabled = false;
	//		}
	//	}		
	//}

	private void ResetRope()
	{
		ropeJoint.enabled = false;
		ropeJoint.maxDistanceOnly = false;
		RopeAttached = false;
		////playerMovement.isSwinging = false;
		//ropeRenderer.positionCount = 2;
		ropeRenderer.SetPosition(0, Origin.transform.position);
		ropeRenderer.SetPosition(1, Origin.transform.position);
		ropeRenderer.enabled = false;
		//ropePositions.Clear();
		//ropeHingeAnchorSprite.enabled = false;
	}

	//private void UpdateRopePositions()
	//{
	//	// 1
	//	if (!ropeAttached)
	//	{
	//		return;
	//	}

	//	// 2
	//	ropeRenderer.positionCount = ropePositions.Count + 1;

	//	// 3
	//	for (var i = ropeRenderer.positionCount - 1; i >= 0; i--)
	//	{
	//		if (i != ropeRenderer.positionCount - 1) // if not the Last point of line renderer
	//		{
	//			ropeRenderer.SetPosition(i, ropePositions[i]);

	//			// 4
	//			if (i == ropePositions.Count - 1 || ropePositions.Count == 1)
	//			{
	//				var ropePosition = ropePositions[ropePositions.Count - 1];
	//				if (ropePositions.Count == 1)
	//				{
	//					ropeHingeAnchorRb.transform.position = ropePosition;
	//					if (!distanceSet)
	//					{
	//						ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
	//						distanceSet = true;
	//					}
	//				}
	//				else
	//				{
	//					ropeHingeAnchorRb.transform.position = ropePosition;
	//					if (!distanceSet)
	//					{
	//						ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
	//						distanceSet = true;
	//					}
	//				}
	//			}
	//			// 5
	//			else if (i - 1 == ropePositions.IndexOf(ropePositions.Last()))
	//			{
	//				var ropePosition = ropePositions.Last();
	//				ropeHingeAnchorRb.transform.position = ropePosition;
	//				if (!distanceSet)
	//				{
	//					ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
	//					distanceSet = true;
	//				}
	//			}
	//		}
	//		else
	//		{
	//			// 6
	//			ropeRenderer.SetPosition(i, transform.position);
	//		}
	//	}
	//}

	public void Shoot(Transform bulletSpawn, Transform hook)
	{
		ropeRenderer.enabled = true;
		Origin = bulletSpawn;
		Hook = hook;
	}

	public void DetachRope()
	{
		if (RopeAttached)
		{
			if (Hook != null)
			{
				//Destroy(Hook.gameObject);
				//ObjectPooler.pooler.PushObject(Hook.gameObject, PoolObjectKey.Hook);
				Hook.GetComponent<HookBehaviour>().DestroyHook();
			}
			ResetRope();
			equipManager.EquippedRanged();
			return;
		}
	}

}
