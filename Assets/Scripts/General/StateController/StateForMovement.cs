using DiContainerLibrary.DiContainer;
using Implementation.Data;
using UnityEngine;

namespace General.State
{
    /// <summary>
    /// Defines stat that is used for movement.
    /// </summary>
    public abstract class StateForMovement : State
    {
        /// <summary>
        /// Gets or sets character movement.
        /// </summary>
        [InjectDiContainter]
        protected IMovementData MovementData { get; set; }

        /// <summary>
        /// Gets or sets rig body component.
        /// </summary>
        protected Rigidbody2D rigBody { get; set; }

        protected override void Initialization_State()
        {
            base.Initialization_State();
            rigBody = GetComponent<Rigidbody2D>();
            MovementData = SaveAndLoadData<IMovementData>.LoadSpecificData(controller.Id);
        }
    }
}
