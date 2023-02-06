using UnityEngine;

public class RollingStoneStop : RollingStone
{  
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.gameObject.tag == "Border")
		{
			controller.EndState(this);
		}
	}

	public override void OnExit_State()
    {
        this.gameObject.layer = 0;
        this.gameObject.tag = "Ground";
        base.OnExit_State();
    }
}
