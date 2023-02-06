using General.State;

namespace CustomCamera.Movement
{
    public class CamStopMoving : StateForMovement
    {
        protected override void Initialization_State()
        {
            base.Initialization_State();
            Priority = 1000;
        }
    }
}
