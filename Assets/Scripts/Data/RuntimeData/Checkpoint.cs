using DiContainerLibrary.DiContainer;
using General.State;
using UnityEngine;

namespace Implementation.Data.Runtime
{
    public class Checkpoint : HighPriorityState
    {
        /// <summary>
        /// Gets or sets enemy data.
        /// </summary>
        [InjectDiContainter]
        private IGameInformation gameInformation { get; set; }

        public override void OnEnter_State()
        {
            base.OnEnter_State();
            CheckpointController.singleton.SaveCheckpoint(transform);
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (controller.ActiveHighPriorityState != this && gameInformation.Player == collision.gameObject)
            {
                controller.ForceSwapState(this);
            }

        }
    }
}
