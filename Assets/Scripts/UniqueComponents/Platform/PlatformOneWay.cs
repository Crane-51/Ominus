using DiContainerLibrary.DiContainer;
using General.Enums;
using General.State;
using Implementation.Data;
using UnityEngine;

namespace UniqueComponent.Platform
{
    public class PlatformOneWay : StateForMovement
    {
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
        /// Gets or sets converted direction.
        /// </summary>
        protected int convertedDirection { get; set; }

        protected override void Initialization_State()
        {
            base.Initialization_State();
            convertedDirection = (int)Direction;
        }

        public override void Update_State()
        {
            if(controller.ActiveStateMovement != this)
            {
                rigBody.velocity = Vector2.zero;
            }
        }

        public override void WhileActive_State()
        {
            if (HorizontalMovement)
            {
                transform.Translate(new Vector2(MovementData.MovementSpeed / 2 * Time.deltaTime * convertedDirection, 0));
            }
            else if (VerticalMovement)
            {
                transform.Translate(new Vector2(0, MovementData.MovementSpeed / 2 * Time.deltaTime * convertedDirection));
            }
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject == gameInformation.Player)
            {
                other.collider.transform.SetParent(transform);
                controller.SwapState(this);
            }
            else
            {
                Debug.Log("Other");
                convertedDirection *= -1;
            }
        }

        void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.collider.transform.SetParent(null);
            }
        }

        public override void OnEnter_State()
        {
            designController.StartTask(this);
            convertedDirection = (int)Direction;
        }

        public override void OnExit_State()
        {
            designController.StopTask(this);
        }
    }
}