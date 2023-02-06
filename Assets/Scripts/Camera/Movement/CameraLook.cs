using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;

namespace CustomCamera
{
	public class CameraLook : StateForMovement
	{
		[InjectDiContainter]
		protected IPlayerKeybindsData keybinds;
		[SerializeField] private float movementSpeed = 3f;
		[InjectDiContainter]
		protected IGameInformation gameInformation { get; set; }
		private Vector3 focusPoint;
		[SerializeField] private float limitAbove = 5f;
		[SerializeField] private float limitBelow = 7f;
		private float movement;

		protected override void Initialization_State()
		{
			base.Initialization_State();
			Priority = 11;
			keybinds = SaveAndLoadData<IPlayerKeybindsData>.LoadSpecificData("Keybinds");
			focusPoint = gameInformation.Player.transform.position;
			focusPoint.y += 2f;
		}

		public override void Update_State()
		{
			base.Update_State();
			movement = (Input.GetKey(keybinds.KeyboardUp) ? 1 : 0) + (Input.GetKey(keybinds.KeyboardDown) ? -1 : 0);
			//Debug.DrawLine(new Vector3(gameInformation.Player.transform.position.x - 4, gameInformation.Player.transform.position.y + limitAbove, 0f),
			//	new Vector3(gameInformation.Player.transform.position.x + 4, gameInformation.Player.transform.position.y + limitAbove, 0f), Color.magenta);
			//Debug.DrawLine(new Vector3(gameInformation.Player.transform.position.x - 4, gameInformation.Player.transform.position.y - limitBelow, 0f),
			//	new Vector3(gameInformation.Player.transform.position.x + 4, gameInformation.Player.transform.position.y - limitBelow, 0f), Color.magenta);
		}

		public override void WhileActive_State()
		{
			base.WhileActive_State();
			if (transform.position.y > gameInformation.Player.transform.position.y - limitBelow && transform.position.y < gameInformation.Player.transform.position.y + limitAbove)
			{
				transform.Translate(0f, movement * movementSpeed * Time.fixedDeltaTime, 0f);
			}

			if (movement == 0)
			{
				controller.EndState(this);
			}
		}

		public void Activate()
		{
			controller.SwapState(this);
		}

		public void Deactivate()
		{
			controller.EndState(this);
		}
	}
}