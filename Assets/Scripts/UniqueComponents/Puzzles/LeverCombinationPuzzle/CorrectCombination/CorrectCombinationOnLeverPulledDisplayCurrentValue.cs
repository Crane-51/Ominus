using General.State;
using UnityEngine;

public class CorrectCombinationOnLeverPulledDisplayCurrentValue : HighPriorityState
{
    public int CorrectValue;
    public int NumberOfValues;

    public int CurrentValue { get; set; }

    protected override void Initialization_State()
    {
        base.Initialization_State();
        CurrentValue = Random.Range(0, NumberOfValues);
        designController.animationController.StartAnimation(CurrentValue.ToString());
    }

    public override void OnEnter_State()
    {
        designController.animationController.StopAnimation(CurrentValue.ToString());
        CurrentValue++;
        CurrentValue %= NumberOfValues;
        designController.animationController.StopAnimation(CurrentValue.ToString());
        designController.animationController.StartAnimation(CurrentValue.ToString());

        controller.EndState(this);
    }

    public override void OnExit_State()
    {
    }

    public void ChangeValue()
    {
        controller.SwapState(this);
    }
}
