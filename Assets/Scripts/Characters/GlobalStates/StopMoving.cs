using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;

namespace Global.Mechanic
{
    public class StopMoving : HighPriorityState
    {
        /// <summary>
        /// Gets or sets game information.
        /// </summary>
        [InjectDiContainter]
        private IGameInformation gameInformation { get; set; }

        private Rigidbody2D rigb { get; set; }

		private Vector3 velocity;

        protected override void Initialization_State()
        {
            base.Initialization_State();
            rigb = GetComponent<Rigidbody2D>();
        }

        public override void OnEnter_State()
        {
            base.OnEnter_State();
			velocity = rigb.velocity;
			//rigb.Sleep();
		}
        public override void Update_State()
        {
            if(controller.ActiveHighPriorityState != this && gameInformation.StopMovement && !(controller.ActiveStateMovement is EnemySleep))
            {
                controller.SwapState(this);
            }
        }

        public override void WhileActive_State()
        {
            base.WhileActive_State();
			rigb.velocity = new Vector3(0f, 0f, 0f);
            if(!gameInformation.StopMovement)
            {
                controller.EndState(this);
            }
        }

        public override void OnExit_State()
        {
            base.OnExit_State();
			rigb.velocity = velocity;
			//rigb.WakeUp();
        }
    }
}
