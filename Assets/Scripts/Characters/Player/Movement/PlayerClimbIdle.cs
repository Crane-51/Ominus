using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;
using Player.Other;

namespace Player.Movement
{
	public class PlayerClimbIdle : PlayerClimbMovement
	{
		private StateForMovement lastStateForMovement;
		bool grounded = false;

		protected override void Initialization_State()
		{
			base.Initialization_State();
			Priority = 1;
		}

		public override void OnEnter_State()
		{
			base.OnEnter_State();
			rigBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
		}

		public override void Update_State()
		{
			if (!grounded && canClimb && !rope.RopeAttached)
			{
				controller.SwapState(this);
			}
		}

		public override void WhileActive_State()
		{
			if (!canClimb || grounded)
			{
				controller.EndState(this);
			}
		}

		public override void OnExit_State()
		{
			base.OnExit_State();
			rigBody.constraints = RigidbodyConstraints2D.FreezeRotation;
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (/*controller.ActiveStateMovement == this &&*/ collision.tag == "Ground")
			{
				grounded = true;
			}
			else if (collision.gameObject.tag == "Climbable")
			{
				canClimb = true;
			}
		}

		private void OnTriggerStay2D(Collider2D collision)
		{
			if (collision.tag == "Ground")
			{
				grounded = true;
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (collision.gameObject.tag == "Climbable")
			{
				canClimb = false;
			}
			else if (collision.tag == "Ground")
			{
				grounded = false;
			}
		}
	}
}
