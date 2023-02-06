using System.Collections.Generic;
using System.Linq;
using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using Player.Movement;
using UnityEngine;

namespace CustomCamera
{
    /// <summary>
    /// Scripts for following the object.
    /// </summary>
    public class CameraFollowObject : StateForMovement
    {
        /// <summary>
        /// Gets or sets enemy data.
        /// </summary>
        [InjectDiContainter]
        protected IGameInformation gameInformation { get; set; }
		[InjectDiContainter]
		protected IPlayerKeybindsData keybinds;

		//[InjectDiContainter]
		///// Holds all value about camera movement.
		//protected ICameraData cameraData { get; set; }

		/// <summary>
		/// Gets or sets object that is being followed by camera.
		/// </summary>
		 [HideInInspector] public Transform ActiveObjectToFollow { get; set; }

		// Use this for initialization
		protected override void Initialization_State()
        {
            base.Initialization_State();
            ActiveObjectToFollow = gameInformation.Player.transform;
			keybinds = SaveAndLoadData<IPlayerKeybindsData>.LoadSpecificData("Keybinds");
			Priority = 10;
			transform.position = new Vector3(ActiveObjectToFollow.position.x, ActiveObjectToFollow.position.y, transform.position.z);
		}

        public override void OnEnter_State()
        {
            base.OnEnter_State();
        }

        public override void Update_State()
        {
            base.Update_State();
			if (controller.ActiveStateMovement != this)
            {
                controller.SwapState(this);
            }
        }

        public override void WhileActive_State()
        {
            if (controller.ActiveStateMovement == this)
            {
                var movementSpeedX = (float)(Mathf.Abs(ActiveObjectToFollow.position.x - transform.position.x));
                var movementSpeedY = (float)(Mathf.Abs(ActiveObjectToFollow.position.y - transform.position.y));
				//var movementSpeedX = (float)(Mathf.Abs(gameInformation.Player.transform.position.x - transform.position.x));
    //            var movementSpeedY = (float)(Mathf.Abs(gameInformation.Player.transform.position.y - transform.position.y));


				if (movementSpeedX > 0.5f)
				{
					transform.position = Vector3.Slerp(transform.position, new Vector3(ActiveObjectToFollow.position.x, transform.position.y, transform.position.z), movementSpeedX * Time.fixedDeltaTime);
					//transform.position = Vector3.Slerp(transform.position, new Vector3(gameInformation.Player.transform.position.x, transform.position.y, transform.position.z), movementSpeedX * Time.fixedDeltaTime);
				}

				transform.position = Vector3.Slerp(transform.position, new Vector3(transform.position.x, ActiveObjectToFollow.position.y + 2, transform.position.z), movementSpeedY * Time.fixedDeltaTime); 
				//transform.position = Vector3.Slerp(transform.position, new Vector3(transform.position.x, gameInformation.Player.transform.position.y + 2, transform.position.z), movementSpeedY * Time.fixedDeltaTime); 
			}
		}
    }
}
