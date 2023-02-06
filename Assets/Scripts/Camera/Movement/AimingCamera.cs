using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;

namespace CustomCamera
{
	public class AimingCamera : StateForMovement
	{
		[InjectDiContainter]
		protected IGameInformation gameInformation { get; set; }
		private float offsetY = 2;
		private float initialOffset = 2;
		private CrosshairMovement crosshair;
		[SerializeField] private float angleThreshold = 15;
		private float lastAngle = 0;
		[SerializeField] private float limitAbove = 5f;
		[SerializeField] private float limitBelow = 7f;
		private float positionY;
		private float movementSpeedX;
		private float movementSpeedY;

		protected override void Initialization_State()
		{
			base.Initialization_State();
			Priority = 12;
		}

		public override void WhileActive_State()
		{
			base.WhileActive_State();
			movementSpeedX = (float)(Mathf.Abs(gameInformation.Player.transform.position.x - transform.position.x));

			if (movementSpeedX > 0.5f)
			{
				transform.position = Vector3.Slerp(transform.position, new Vector3(gameInformation.Player.transform.position.x, transform.position.y, transform.position.z), movementSpeedX * Time.fixedDeltaTime);
			}

			if (crosshair.Angle > angleThreshold /*&& crosshair.UpDownIndicator == crosshair.MovementDirection*/)
			{
				if (Mathf.Abs(lastAngle - crosshair.Angle) > 0.01f)
				{
					float limit;
					switch (crosshair.UpDownIndicator)
					{
						case 1:
						{
							limit = limitAbove;
							break;
						}
						case -1:
						{
							limit = limitBelow;
							break;
						}
						default:
						{
							limit = 0;
							break;
						}
					}
					offsetY = (limit * (crosshair.Angle - angleThreshold)) / (crosshair.MaxAngle - angleThreshold);
				}
			}
			else
			{
				if (crosshair.Angle < angleThreshold)
				{
					offsetY = initialOffset;
				}
			}

			positionY = gameInformation.Player.transform.position.y + offsetY * crosshair.UpDownIndicator;
			movementSpeedY = (float)(Mathf.Abs(positionY - transform.position.y));
			transform.position = Vector3.Slerp(transform.position, new Vector3(transform.position.x, positionY, transform.position.z), movementSpeedY * Time.fixedDeltaTime);			
		}

		public void Activate(CrosshairMovement ch)
		{
			crosshair = ch;
			lastAngle = crosshair.Angle;
			controller.SwapState(this);
		}

		public void Deactivate()
		{
			controller.EndState(this);
		}
	}
}