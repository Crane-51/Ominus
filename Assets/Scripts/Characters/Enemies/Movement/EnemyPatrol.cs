using System.Collections;
using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;

namespace Enemy.State
{
    /// <summary>
    /// Holds implementation of enemy patrol, while <see cref="GlobalGameInformation.Alarmed"/> is false.
    /// </summary>
    public class EnemyPatrol : StateForMovement
    {
        /// <summary>
        /// Gets or sets time of waiting.
        /// </summary>
        [SerializeField] private float timeOfWaiting;

		/// <summary>
		/// Gets or sets direction correction (direction of movement).
		/// </summary>
		private int directionCorrection;

		/// <summary>
		/// Gets or sets value indicating if enemy should stop moving.
		/// </summary>
		private bool stopMoving;

        protected override void Initialization_State()
        {
            base.Initialization_State();
            Priority = -10;
            stopMoving = false;
            directionCorrection = (int)transform.localScale.x;
        }

        public override void OnEnter_State()
        {
            base.OnEnter_State();
            stopMoving = false;
        }

        public override void Update_State()
        {
            base.Update_State();
            if (controller.ActiveStateMechanic == null && !stopMoving)
            {
                controller.SwapState(this);
            }
        }

        public override void WhileActive_State()
        {
            base.WhileActive_State();

            if (controller.ActiveStateMechanic != null || stopMoving)
            {
                controller.EndState(this);
            }
			else
			{
				rigBody.velocity = new Vector2(transform.localScale.x * 20 * Time.deltaTime * MovementData.MovementSpeed, rigBody.velocity.y);
				this.transform.localScale = new Vector3(directionCorrection, 1, 1);

				RaycastHit2D wallHit =
						Physics2D.Raycast(transform.position, transform.localScale.x == -1 ? Vector3.left : Vector3.right, 1.5f, LayerMask.GetMask("Environment", "Border"));
				//Debug.DrawRay(transform.position, (transform.localScale.x == -1 ? Vector3.left : Vector3.right) * 1.5f, Color.magenta);
				if (wallHit.collider)
				{
					rigBody.velocity = new Vector2(0, rigBody.velocity.y);
					StartCoroutine(WaitBeforeTurning());
					controller.EndState(this);
				}
			}
        }

        private IEnumerator WaitBeforeTurning()
        {
            stopMoving = true;
            yield return new WaitForSeconds(timeOfWaiting);
            directionCorrection = -directionCorrection;
            stopMoving = false;
        }
	}
}
