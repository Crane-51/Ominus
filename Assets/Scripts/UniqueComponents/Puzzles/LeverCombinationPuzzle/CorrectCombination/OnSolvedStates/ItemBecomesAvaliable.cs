using UniqueComponent.Puzzles;
using UnityEngine;

public class ItemBecomesAvaliable : OnSolved
{
    private BoxCollider2D triggerActivate { get; set; }

    protected override void Initialization_State()
    {
        base.Initialization_State();
        triggerActivate = GetComponent<BoxCollider2D>();
        triggerActivate.enabled = false;		//NP comment: shouldn't the whole object be enabled/disabled instead of just the collider?
    }
    public override void OnEnter_State()
    {
        base.OnEnter_State();
        triggerActivate.enabled = true;
    }
}
