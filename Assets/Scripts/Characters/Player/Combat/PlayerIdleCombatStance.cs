using System.Collections;
using General.State;
using UnityEngine;

public class PlayerIdleCombatStance : StateForMovement
{
    private IEnumerator stanceStandTimer { get; set; }

    protected override void Initialization_State()
    {
        base.Initialization_State();
        Priority = -9;
    }
    public override void OnEnter_State()
    {
        stanceStandTimer = StanceStandTimer();
        base.OnEnter_State();
        StartCoroutine(stanceStandTimer);
    }

    public override void WhileActive_State()
    {
        base.WhileActive_State();
		rigBody.velocity = new Vector2(0, 0/*rigBody.velocity.y*/);
	}

    public override void OnExit_State()
    {
		designController.animationController.Anima.SetBool("PlayerIdleCombatStance", false);
		base.OnExit_State();
        StopCoroutine(stanceStandTimer);
		//designController.animationController.Anima.SetBool("PlayerIdleCombatStance", false);
	}

    private IEnumerator StanceStandTimer()
    {
        yield return new WaitForSeconds(5);
        controller.EndState(this);
    }
}
