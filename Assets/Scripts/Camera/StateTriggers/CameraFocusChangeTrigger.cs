using CustomCamera.Movement;
using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;

namespace CustomCamera.StateTriggers
{ 
    public class CameraFocusChangeTrigger : HighPriorityState
    {
        /// <summary>
        /// Focus point of camera.
        /// </summary>
        public Transform FocusPoint;

        /// <summary>
        /// Duration of focus
        /// </summary>
        public float Duration;

        /// <summary>
        /// Movement speed of camera.
        /// </summary>
        public float MovementSpeed;

        /// <summary>
        /// Gets or sets enemy data.
        /// </summary>
        [InjectDiContainter]
        protected IGameInformation gameInformation { get; set; }

        private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
        {
            if(collision.gameObject == gameInformation.Player)
            {
                CameraChangeFocus.singleton.StartState(FocusPoint.transform.position, Duration, MovementSpeed);
                Destroy(this.gameObject);
            }
        }
    }
}
