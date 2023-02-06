using UnityEngine;
using Player.Other;
using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using CustomCamera;

namespace Player.Movement
{
	public class PlayerLook : StateForMovement
	{
		/// <summary>
		/// Gets or sets player key binds;
		/// </summary>
		[InjectDiContainter]
		protected IPlayerKeybindsData keybinds;
		[InjectDiContainter]
		protected IGameInformation gameInformation { get; set; }
		private CameraLook camLook;
		private int keyPressed = 0;
		private EquipmentManager equipManager;
		//[SerializeField] private float upperAngleThreshold;
		//[SerializeField] private float lowerAngleThreshold;

		protected override void Initialization_State()
		{
			base.Initialization_State();
			Priority = -8;
			keybinds = SaveAndLoadData<IPlayerKeybindsData>.LoadSpecificData("Keybinds");
			camLook = gameInformation.Camera.gameObject.GetComponent<CameraLook>();			
			equipManager = GetComponent<EquipmentManager>();
		}

		public override void Update_State()
		{
			base.Update_State();
			keyPressed = (Input.GetKey(keybinds.KeyboardDown) ? -1 : 0) + (Input.GetKey(keybinds.KeyboardUp) ? 1 : 0);
			// maybe check != climbing to avoid pointless multiple swaps
			if (controller.ActiveStateMovement != this && keyPressed != 0 && !equipManager.Crosshair.IsEnabled())
				//|| (equipManager.Crosshair.Angle > upperAngleThreshold && equipManager.Crosshair.UpDownIndicator == keyPressed)))																	
			{				
				controller.SwapState(this);
			}
		}

		public override void OnEnter_State()
		{
			base.OnEnter_State();
			if (camLook != null)
			{
				camLook.Activate();
				rigBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
			}
			else
			{
				Debug.LogWarning("Error finding camera look");
			}
		}

		public override void WhileActive_State()
		{
			base.WhileActive_State();
			if (!equipManager.Crosshair.IsEnabled() && keyPressed == 0)
			{
				controller.EndState(this);
			}
		}

		public override void OnExit_State()
		{
			base.OnExit_State();
			if (camLook != null)
			{
				camLook.Deactivate();
				rigBody.constraints = RigidbodyConstraints2D.FreezeRotation;
			}
			else
			{
				Debug.LogWarning("Error finding camera look");
			}
		}
	}
}