using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Movement;
using Implementation.Data;
using General.State;
using DiContainerLibrary.DiContainer;

public class PlayerDodge : StateForMovement
{
	[InjectDiContainter]
	protected IPlayerKeybindsData keybinds { get; set; }
	private const float offsetOnY = -0.84f;
	private const float sizeOfY = 1.11f;
	private float originalOffsetOnY;
	private float originalSizeOfY;
	private CapsuleCollider2D capsuleCollider;
	private bool tmpFlag = false;

	protected override void Initialization_State()
	{
		base.Initialization_State();
		Priority = -3;
		keybinds = SaveAndLoadData<IPlayerKeybindsData>.LoadSpecificData("Keybinds");
		capsuleCollider = GetComponent<CapsuleCollider2D>();
		originalOffsetOnY = capsuleCollider.offset.y;
		originalSizeOfY = capsuleCollider.size.y;
	}

	public override void Update_State()
	{
		MovementData.HorizontalMovement = (Input.GetKey(keybinds.KeyboardRight) ? 1 : 0) + (Input.GetKey(keybinds.KeyboardLeft) ? -1 : 0);

		if (controller.ActiveStateMovement != this && Input.GetKeyDown(keybinds.KeyboardDodge) && MovementData.HorizontalMovement != 0)
		{
			controller.SwapState(this);
		}
	}

	public override void OnEnter_State()
	{
		base.OnEnter_State();
		MovementData.MovementSpeed *= 2;
		capsuleCollider.offset = new Vector2(capsuleCollider.offset.x, offsetOnY);
		capsuleCollider.size = new Vector2(capsuleCollider.size.x, sizeOfY);
		tmpFlag = false;
		StartCoroutine(WaitForDodgingEnd());
	}

	public override void WhileActive_State()
	{		
		base.WhileActive_State();
		rigBody.velocity = new Vector2(MovementData.HorizontalMovement * MovementData.MovementSpeed, rigBody.velocity.y);
		if (tmpFlag)
		{
			controller.EndState(this);
		}
	}

	public override void OnExit_State()
	{
		base.OnExit_State();
		MovementData.MovementSpeed /= 2;
		capsuleCollider.offset = new Vector2(capsuleCollider.offset.x, originalOffsetOnY);
		capsuleCollider.size = new Vector2(capsuleCollider.size.x, originalSizeOfY);
	}

	private IEnumerator WaitForDodgingEnd()
	{
		yield return new WaitForSeconds(0.5f);
		tmpFlag = true;
	}
}
