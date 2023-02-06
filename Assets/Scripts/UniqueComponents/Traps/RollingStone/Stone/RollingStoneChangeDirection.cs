using General.State;
using UnityEngine;

public class RollingStoneChangeDirection : HighPriorityState
{
    [SerializeField] private float RollingForce;
	[SerializeField] private float MaxAllowedForce;
	[SerializeField] private float MovementSpeed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var canChangeDirection = collision.gameObject.GetComponent<RollingStone>();

        if(canChangeDirection != null)
        {
            canChangeDirection.ChangeDirection(RollingForce, MaxAllowedForce, MovementSpeed);
        }
    }
}
