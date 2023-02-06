using System.Collections.Generic;
using System.Linq;
using General.State;
using UniqueComponent.Puzzles;

public class CorrectCombinationOnLeverPulledOnSolved : HighPriorityState
{
    private List<CorrectCombinationOnLeverPulled> correctCombination { get; set; }

    private List<OnSolved> OnSolveds { get; set; }

    protected override void Initialization_State()
    {
        base.Initialization_State();
        correctCombination = GetComponentsInChildren<CorrectCombinationOnLeverPulled>().ToList();
        OnSolveds = GetComponentsInChildren<OnSolved>().ToList();

        controller.SwapState(this);
    }

    public override void WhileActive_State()
    {
        if(correctCombination.All(x=> x.IsInCorrectState()))
        {
            foreach(var obj in OnSolveds)
            {
                obj.PuzzleSolved();
            }

            controller.EndState(this);
        }
    }
}
