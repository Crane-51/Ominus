using General.State;
using UnityEngine;

public class GroundThatMovesUp : HighPriorityState
{
    public override void WhileActive_State()
    {
        base.WhileActive_State();
        transform.Translate(new Vector2(0, 10 * Time.deltaTime));
    }

    public void SwapToThisState()
    {
        controller.SwapState(this);
    }
}
