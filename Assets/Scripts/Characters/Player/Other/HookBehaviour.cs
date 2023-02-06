using UnityEngine;
using DiContainerLibrary.DiContainer;
using Implementation.Data;
using General.State;
using General.Enums;

public class HookBehaviour : HighPriorityState
{
	private LineRenderer lr;
	private Rigidbody2D rb;
	private Transform origin;
	Vector3 position;
	public bool RopeAttached { get; private set; }
	public AnchorType Anchor { get; private set; }
	private FixedJoint2D joint;

	//protected override void Initialization_State()
	//{
	//	base.Initialization_State();
	//	lr = GetComponent<LineRenderer>();
	//	rb = GetComponent<Rigidbody2D>();
	//}

	//public override void WhileActive_State()
	//{
	//	base.WhileActive_State();
	//	position = origin.position;
	//	position.z = -1;
	//	lr.SetPosition(0, position);
	//	position = transform.position;
	//	position.z = -1;
	//	lr.SetPosition(1, position);
	//}

	public void SetOriginPosition(Transform bulletSpawnTransform)
	{
		base.Initialization_State();
		//lr = GetComponent<LineRenderer>();
		rb = GetComponent<Rigidbody2D>();
		joint = GetComponent<FixedJoint2D>();
		//origin = bulletSpawnTransform;
		//controller.SwapState(this);
	}

	public void DestroyHook()
	{
		RopeAttached = false;
		rb.bodyType = RigidbodyType2D.Dynamic;
		joint.enabled = false;
		joint.connectedBody = null;
		ObjectPooler.pooler.PushObject(gameObject, PoolObjectKey.Hook);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Anchor")
		{
			//rb.velocity = new Vector2(0f, 0f);
			RopeAttached = true;			
			Anchor = collision.gameObject.GetComponent<HookAnchor>().type;
			if (Anchor == AnchorType.Swing)
			{
				rb.bodyType = RigidbodyType2D.Static;
			}
			else
			{
				rb.velocity = new Vector2(0f, 0f);
				joint.connectedBody = collision.GetComponent<Rigidbody2D>();
				joint.enabled = true;
			}
			//Debug.Log(Anchor);
		}
		else if (collision.tag == "Ground")
		{
			//Destroy(this.gameObject);
			//ObjectPooler.pooler.PushObject(gameObject, PoolObjectKey.Hook);
			DestroyHook();
		}
	}
}
