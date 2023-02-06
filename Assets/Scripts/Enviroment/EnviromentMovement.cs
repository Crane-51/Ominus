using DiContainerLibrary.DiContainer;
using General.Enums;
using General.State;
using Implementation.Custom;
using Implementation.Data;
using UnityEngine;

namespace Enviroment.Movement
{
    public class EnviromentMovement : HighPriorityState, IActivate
    {
        /// <summary>
        /// Defines if state will be started imidietly or not.
        /// </summary>
        public bool StartAwake;

        /// <summary>
        /// Defines if movement is single line or loop.
        /// </summary>
        public bool Loop;

        /// <summary>
        /// Gets or sets enemy data.
        /// </summary>
        [InjectDiContainter]
        protected IGameInformation gameInformation { get; set; }
        /// <summary>
        /// Is platform moving horizontally.
        /// </summary>
        public bool HorizontalMovement;

        /// <summary>
        /// Is platform moving vertically.
        /// </summary>
        public bool VerticalMovement;

        /// <summary>
        /// Direction of the platform.
        /// </summary>
        public DirectionEnum Direction;

        /// <summary>
        /// Defines movement speed;
        /// </summary>
        public float MovementSpeed;

        /// <summary>
        /// Gets or sets converted direction.
        /// </summary>
        protected int convertedDirection { get; set; }
		protected bool isActivated = false;

        protected override void Initialization_State()
        {
            base.Initialization_State();
            convertedDirection = (int)Direction;

            if(StartAwake)
            {
                controller.SwapState(this);
            }

            Priority = 10;
        }

		public override void Update_State()
		{
			base.Update_State();
			if (controller.ActiveHighPriorityState != this && !gameInformation.StopMovement && (StartAwake || isActivated))
			{
				controller.SwapState(this);
			}
		}

		public override void WhileActive_State()
        {
            if (HorizontalMovement)
            {
                transform.Translate(new Vector2(MovementSpeed * Time.deltaTime * convertedDirection, 0));
            }
            else if (VerticalMovement)
            {
                transform.Translate(new Vector2(0, MovementSpeed * Time.deltaTime * convertedDirection));
            }

			if (gameInformation.StopMovement)
			{
				controller.EndState(this);
			}
        }

        public virtual void Activate()
        {
			isActivated = true;
            controller.SwapState(this);
        }
    }
}
