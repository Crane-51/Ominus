using UnityEngine;

namespace General.State
{
    public class DesignController : MonoBehaviour
    {
        /// <summary>
        /// Gets or sets sound controller.
        /// </summary>
        public  SoundController soundController { get; private set; }

        /// <summary>
        /// Gets or sets animation controller.
        /// </summary>
        public  AnimationController animationController { get; private set; }

        private void Awake()
        {
            soundController = GetComponent<SoundController>();
            animationController = GetComponent<AnimationController>();
        }

        public void StartTask(State state)
        {
            if (state == null)
				return;

            if (animationController != null)
            {
                animationController.StartAnimation(state.GetType().Name);
            }

            if (soundController != null)
            {
                soundController.PlaySound(state);
            }
        }

        public void StopTask(State state)
        {
            if (state == null)
				return;

            if (animationController != null)
            {
                animationController.StopAnimation(state.GetType().Name);
            }

            if (soundController != null)
            {
                soundController.StopSound(state);
            }
        }
    }
}
