using System.Collections;
using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;

namespace CustomCamera.Movement
{
    public class CameraChangeFocus : HighPriorityState
    {
        public static CameraChangeFocus singleton { get; set; }

        /// <summary>
        /// Gets or sets enemy data.
        /// </summary>
        [InjectDiContainter]
        protected IGameInformation gameInformation { get; set; }

        /// <summary>
        /// Gets or sets focus point.
        /// </summary>
        private Vector2 focusPoint { get; set; }

        /// <summary>
        /// Gets or sets duration of state.
        /// </summary>
        private float duration { get; set; }

        /// <summary>
        /// Gets or sets movement speed of camera.
        /// </summary>
        private float movementSpeed { get; set; }

        protected override void Initialization_State()
        {
            base.Initialization_State();
            singleton = this;
            Priority = 100;
        }

        public override void WhileActive_State()
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(focusPoint.x,focusPoint.y, transform.position.z), movementSpeed * Time.deltaTime);
        }

        public override void OnExit_State()
        {
            base.OnExit_State();
            focusPoint = Vector2.zero;
            duration = 0;
            transform.position = new Vector3(gameInformation.Player.transform.position.x, gameInformation.Player.transform.position.y, transform.position.z);
        }

        public void StartState(Vector2 focusPoint, float duration, float movementSpeed)
        {
            this.focusPoint = focusPoint;
            this.duration = duration;
            this.movementSpeed = movementSpeed;
            StartCoroutine(stateDuration());
        }

        private IEnumerator stateDuration()
        {
            controller.SwapState(this);
            yield return new WaitForSeconds(duration);
            controller.EndState(this);
        }
    }
}
