using UniqueComponent.Puzzles;
using UnityEngine;

public class OpenLockedGates : OnSolved
{
    private Collider2D colliderToActivate { get; set; }

    protected override void Initialization_State()
    {
        base.Initialization_State();
        colliderToActivate = this.GetComponent<Collider2D>();
        colliderToActivate.enabled = false;		//NP comment: shouldn't this be the other way around?
    }

    public override void OnEnter_State()
    {
        colliderToActivate.enabled = true;
        controller.EndState(this);
    }
}
