using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General.State;
using Implementation.Data;
using DiContainerLibrary.DiContainer;

public class WaypointPlatform : StateForMovement
{
	[SerializeField] private Transform[] waypoints;
	[SerializeField] private float speed;
	[Tooltip("Target Waypoint")] [SerializeField] private int target = 0;
	[InjectDiContainter]
	protected IGameInformation gameInformation { get; set; }

	public override void Update_State()
	{
		base.Update_State();
		if (controller.ActiveStateMovement != this && !gameInformation.StopMovement)
		{
			controller.SwapState(this);
		}
	}

	public override void WhileActive_State()
	{
		base.WhileActive_State();
		transform.position = Vector2.MoveTowards(transform.position, waypoints[target].position, speed * Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Waypoint") && collision.gameObject.transform == waypoints[target])
		{
			target++;
			target = target % waypoints.Length;
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (gameInformation == null) return;
		if (other.gameObject == gameInformation.Player)
		{
			other.collider.transform.SetParent(transform);
		}
	}

	void OnCollisionExit2D(Collision2D other)
	{
		if (other.gameObject == gameInformation.Player)
		{
			other.collider.transform.SetParent(null);
		}
	}
}
