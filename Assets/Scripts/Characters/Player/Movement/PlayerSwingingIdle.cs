using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Other;
using General.Enums;

public class PlayerSwingingIdle : PlayerSwinging
{
	private float originalDrag;
	[SerializeField] private float swingingDrag = 1f;

	protected override void Initialization_State()
	{
		base.Initialization_State();
		Priority = -1;
		originalDrag = rigBody.drag;
	}

	public override void Update_State()
	{
		if (controller.ActiveStateMovement != this && joint.enabled && rope.Anchor == AnchorType.Swing)
		{
			controller.SwapState(this);
		}
	}

	public override void OnEnter_State()
	{
		oppositeForce = swingForce;
		rigBody.drag = swingingDrag;
		if (equipManager.EquippedItem != null && equipManager.EquippedItem.name != "Fists")
		{
			equipManager.EquippedItem.HideSprite();
		}
	}

	public override void WhileActive_State()
	{
		if (!joint.enabled)
		{
			controller.EndState(this);
		}
		//rigBody.velocity -= Vector2.Perpendicular(rope.ropeDirection) * MovementData.GravityEqualizator * MovementData.Gravity * Time.deltaTime;
		//AddOppositeForce();
	}

	public override void OnExit_State()
	{
		base.OnExit_State();
		rigBody.drag = originalDrag;
	}
}
