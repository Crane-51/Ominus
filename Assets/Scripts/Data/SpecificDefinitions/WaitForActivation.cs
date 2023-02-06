using General.State;

namespace Implementation.Custom
{
    public class WaitForActivation : StateForMovement, IActivate
    {
        protected override void Initialization_State()
        {
            base.Initialization_State();
            Priority = 11;
            controller.SwapState(this);
        }

        public void Activate()
        {
            controller.EndState(this);
        }
    }
}
