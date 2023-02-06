using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General.State;
using DiContainerLibrary.DiContainer;
using Implementation.Data;
using Player.Other;
using General.Enums;

public class PlayerSwinging : StateForMovement
{
	protected RopeSystem rope;
	[InjectDiContainter]
	protected IPlayerKeybindsData keybinds;
	protected DistanceJoint2D joint;
 	[SerializeField] protected float swingForce = 2f;
	[SerializeField] protected float oppositeForce = 1f;
	public bool IsSwinging { get; private set; }
	protected EquipmentManager equipManager;

	protected override void Initialization_State()
	{
		base.Initialization_State();
		Priority = 0;
		rope = GetComponentInChildren<RopeSystem>();
		joint = GetComponent<DistanceJoint2D>();
		keybinds = SaveAndLoadData<IPlayerKeybindsData>.LoadSpecificData("Keybinds");
		equipManager = GetComponent<EquipmentManager>();
	}

	public override void Update_State()
	{
		base.Update_State();
		MovementData.VerticalMovement = (Input.GetKey(keybinds.KeyboardUp) ? 1 : 0) + (Input.GetKey(keybinds.KeyboardDown) ? -1 : 0);
		MovementData.HorizontalMovement = (Input.GetKey(keybinds.KeyboardLeft) ? -1 : 0) + (Input.GetKey(keybinds.KeyboardRight) ? 1 : 0);
		if (controller.ActiveStateMovement != this && rope.RopeAttached && rope.Anchor == AnchorType.Swing && (MovementData.VerticalMovement != 0 || MovementData.HorizontalMovement != 0))
		{
			Debug.Log(rope.Anchor);
			Debug.Log(rope.Anchor == AnchorType.Swing);
			controller.SwapState(this);
		}
		if (IsSwinging && !rope.RopeAttached)
		{
			IsSwinging = false;
			PlayerGravity.GroundedTrigger("Swinging", false);
		}
	}

	public override void OnEnter_State()
	{
		base.OnEnter_State();
		joint.connectedBody = rope.Hook.gameObject.GetComponent<Rigidbody2D>();
		//if (MovementData.VerticalMovement != 0)
		if (!IsSwinging && (!PlayerGravity.IsGrounded || PlayerGravity.IsGrounded && MovementData.VerticalMovement != 0))
		{
			joint.distance = Vector2.Distance(transform.position, rope.Hook.position);
			IsSwinging = true;
			PlayerGravity.GroundedTrigger("Swinging", true);
			joint.enabled = true;
		}
		else if (!IsSwinging && PlayerGravity.IsGrounded)
		{
			controller.EndState(this);
		}

		if (equipManager.EquippedItem != null && equipManager.EquippedItem.name != "Fists")
		{
			equipManager.EquippedItem.HideSprite();
		}
	}

	public override void WhileActive_State()
	{
		base.WhileActive_State();
		if ((MovementData.VerticalMovement == 0 && MovementData.HorizontalMovement == 0) || !rope.RopeAttached)
		{
			controller.EndState(this);
			return;
		}

		if (MovementData.VerticalMovement != 0)
		{
			joint.distance += MovementData.VerticalMovement * Time.deltaTime * MovementData.MovementSpeed * (-1); 
		}

		if (MovementData.HorizontalMovement != 0)
		{
			Debug.DrawRay(transform.position, Vector2.Perpendicular(rope.RopeDir), Color.green);
			rigBody.AddForce(swingForce * MovementData.HorizontalMovement * Vector2.Perpendicular(rope.RopeDir), ForceMode2D.Force);
			//rigBody.AddForce(swingForce * MovementData.HorizontalMovement * transform.right, ForceMode2D.Force);
		}
		//rigBody.velocity -= new Vector2(0, MovementData.GravityEqualizator * MovementData.Gravity * Time.deltaTime);
	}

	public override void OnExit_State()
	{
		base.OnExit_State();
		if (equipManager.EquippedItem != null && equipManager.EquippedItem.name != "Fists")
		{
			equipManager.EquippedItem.ShowSprite();
		}
	}

	protected int CheckPosition()
	{
		float diff = transform.position.x - joint.connectedBody.transform.position.x;
		if (diff > 0.1)
		{
			return 1;
		}
		else if (diff < -0.1)
		{
			return -1;
		}
		else
		{
			return 0;
		}
	}

	protected void AddOppositeForce()
	{
		//rigBody.velocity -= new Vector2(0, MovementData.GravityEqualizator * MovementData.Gravity * Time.deltaTime);
		int relativePosition = CheckPosition();
		switch (relativePosition)
		{
			case 1:
			{
				rigBody.velocity -= new Vector2(oppositeForce * Time.deltaTime, 0);
				//rigBody.AddForce(new Vector2(-oppositeForce, 0f), ForceMode2D.Force);
				//rigBody.velocity = new Vector2(-rigBody.velocity.x, rigBody.velocity.y);
				break;
			}
			case 0:
			{
				oppositeForce -= 0.1f;
				if (oppositeForce < 0)
				{
					oppositeForce = 0;
					rigBody.velocity = new Vector2(0f, rigBody.velocity.y);
				}
				else
				{
					rigBody.velocity = new Vector2(rigBody.velocity.x / 1.25f, rigBody.velocity.y);
				}
				Debug.Log(oppositeForce);
				//rigBody.AddForce(new Vector2(0f, swingForce), ForceMode2D.Force);
				
				break;
			}
			case -1:
			{
				rigBody.AddForce(new Vector2(oppositeForce, 0f), ForceMode2D.Force);
				//rigBody.velocity += new Vector2(oppositeForce * Time.deltaTime, 0);
				//rigBody.velocity = new Vector2(-rigBody.velocity.x, rigBody.velocity.y);
				break;
			}
		}
		//Debug.Log(rigBody.velocity);
		//rigBody.AddForce(new Vector2(oppositeForce * Time.deltaTime, 0f), ForceMode2D.Force);
	}
}
