using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using General.Enums;

public class PlayerHookPull : StateForMovement
{
	[InjectDiContainter]
	private IPlayerKeybindsData keybinds;
	private RopeSystem rope;
	private DistanceJoint2D joint;
	private RaycastHit2D hit;

	protected override void Initialization_State()
	{
		base.Initialization_State();
		Priority = -2;
		keybinds = SaveAndLoadData<IPlayerKeybindsData>.LoadSpecificData("Keybinds");
		rope = GetComponentInChildren<RopeSystem>();
		joint = GetComponent<DistanceJoint2D>();
	}

	public override void Update_State()
	{
		base.Update_State();
		MovementData.HorizontalMovement = (Input.GetKey(keybinds.KeyboardLeft) ? -1 : 0) + (Input.GetKey(keybinds.KeyboardRight) ? 1 : 0);
		if (controller.ActiveStateMovement != this && rope.RopeAttached && rope.Anchor == AnchorType.Pull && MovementData.HorizontalMovement != 0)
		{
			//Debug.Log("I'm puuuulling");
			controller.SwapState(this);
		}
	}

	public override void OnEnter_State()
	{
		base.OnEnter_State();
		//Debug.Log("Entering the pull");
		/*
		 * set connected body to anchor
		 * set distance as current distance
		 * enable distance joint on player
		 * */
		joint.connectedBody = rope.Hook.gameObject.GetComponent<Rigidbody2D>();
		joint.distance = Vector2.Distance(transform.position, rope.Hook.position);
		joint.maxDistanceOnly = true;
		joint.enabled = true;		
	}

	public override void WhileActive_State()
	{
		base.WhileActive_State();
		//velocity = MovementData.HorizontalMovement * MovementData.MovementSpeed / 2f;
		//grabbedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity, rigBody.velocity.y);
		rigBody.velocity = new Vector2(MovementData.HorizontalMovement * MovementData.MovementSpeed / 2f, rigBody.velocity.y);
		hit = Physics2D.Raycast(rope.Origin.position, -rope.RopeDir, Vector2.Distance(rope.Origin.position, rope.Hook.position), LayerMask.GetMask("Environment"));
		//Debug.DrawRay(rope.Origin.position, -rope.RopeDir * Vector2.Distance(rope.Origin.position, rope.Hook.position), Color.yellow);
		if (!rope.Hook.gameObject.activeInHierarchy || hit)
		{
			if (hit)
			{
				rope.DetachRope();
			}
			controller.EndState(this);
		}
	}
}
