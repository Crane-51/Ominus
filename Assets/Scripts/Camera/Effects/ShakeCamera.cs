using System.Collections;
using UnityEngine;

namespace CustomCamera
{
    public class ShakeCamera : CameraFollowObject
    {
        /// <summary>
        /// Acces this effect.
        /// </summary>
        public static ShakeCamera singleton;

        /// <summary>
        /// Gets or sets shake offset.
        /// </summary>
        private float shakeOffset;

        /// <summary>
        /// Defines shake offset on Y axis.
        /// </summary>
        private float currentShakeOffset { get; set; }

        /// <summary>
        /// Gets or sets shake duration.
        /// </summary>
        private float shakeDuration { get; set; }

        /// <summary>
        /// Gets or sets direction of shake.
        /// </summary>
        private int shakeDirection { get; set; }


        protected override void Initialization_State()
        {
            base.Initialization_State();
            singleton = this;
            Priority = 11;
            shakeDirection = 1;
        }
        public override void OnEnter_State()
        {
            base.OnEnter_State();
            currentShakeOffset = 0;
            StartCoroutine(shakeTimer());
        }

        public override void Update_State()
        {
        }

        public override void WhileActive_State()
        {
            base.WhileActive_State();
            if(currentShakeOffset < shakeOffset)
            {
                currentShakeOffset += shakeOffset * Time.deltaTime;
            }
            transform.position = new Vector3(transform.position.x, transform.position.y + (shakeDirection * currentShakeOffset), transform.position.z);
            shakeDirection *= -1;
        }

        public void StartShaking(float shakeOffset)
        {
            this.shakeOffset = shakeOffset;

            if (controller.ActiveStateMovement != this)
            {
                controller.SwapState(this);
            }
        }

        public void StopShaking()
        {
            controller.EndState(this);
        }

        public IEnumerator shakeTimer()
        {
            yield return new WaitForSeconds(shakeDuration);
            controller.EndState(this);
        }
    }
}
