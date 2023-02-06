using DiContainerLibrary.DiContainer;
using General.Enums;
using General.State;
using Implementation.Data;
using UnityEngine;

public class RotatingPlatform : HighPriorityState
{
    [SerializeField] private DirectionEnum direction;
	[SerializeField] private Transform point;
	[SerializeField] private float radius;
	[SerializeField] private float offset;
	[SerializeField] private float force;

    [InjectDiContainter]
    private IGameInformation gameInformation { get; set; }
    private int dir { get; set; }

    protected override void Initialization_State()
    {
        base.Initialization_State();
		Priority = -1;
        dir = (int)direction;
        controller.SwapState(this);
    }

	public override void Update_State()
	{
		base.Update_State();
		if (controller.ActiveHighPriorityState != this && !gameInformation.StopMovement)
		{
			controller.SwapState(this);
		}
	}

	public override void WhileActive_State()
    {
        this.offset += dir * force * Time.deltaTime;

        var offset = new Vector2(Mathf.Sin(this.offset), Mathf.Cos(this.offset)) * radius;
        transform.position = new Vector2(point.position.x + offset.x, point.position.y + offset.y);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject == gameInformation.Player)
        {
            other.collider.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.collider.transform.SetParent(null);
        }
    }
}
