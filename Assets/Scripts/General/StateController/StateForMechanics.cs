using DiContainerLibrary.DiContainer;
using Implementation.Data;

namespace General.State
{
    /// <summary>
    /// Defines states that are used for mechanics.
    /// </summary>
    public abstract class StateForMechanics : State
    {
        [InjectDiContainter]
        protected IMechanicsData MechanicsData;
    }
}
