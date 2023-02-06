using UnityEngine;
using DiContainerLibrary.DiContainer;
using Implementation.Data;
using General.State;
using System.Collections;

public class PlatformSwingCollider : HighPriorityState
{
	private bool playerOnPlatform = false;
	[InjectDiContainter]
	private IGameInformation gameInformation { get; set; }

	// Start is called before the first frame update
	//void Start()
	//   {

	//   }

	public override void Update_State()
	{
		base.Update_State();
		if (/*offsetDone &&*/ controller.ActiveHighPriorityState != this && !gameInformation.StopMovement)
		{
			controller.SwapState(this);
		}
		else if (playerOnPlatform && Input.anyKeyDown)
		{
			gameInformation.Player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
		}
		else if (playerOnPlatform && !Input.anyKey)
		{
			gameInformation.Player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
		}
	}

	public override void WhileActive_State()
	{
		base.WhileActive_State();
		if (playerOnPlatform)
		{
			gameInformation.Player.transform.rotation = Quaternion.identity;
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player" && (other.gameObject.transform.position.y - transform.position.y >= 0))
		{
			other.collider.transform.SetParent(transform.parent);
			gameInformation.Player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
			playerOnPlatform = true;
		}
	}

	void OnCollisionExit2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player" && playerOnPlatform)
		{
			other.collider.transform.SetParent(null);
			gameInformation.Player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
			playerOnPlatform = false;
		}

	}
}
