using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using Player.Other;
using UnityEngine;
using Character.Stats;

namespace Player.Movement
{
	/// <summary>
	/// Class that defines state of jumping.
	/// </summary>
	public class PlayerJump : StateForMovement
	{
		/// <summary>
		/// Gets or sets player key binds;
		/// </summary>
		[InjectDiContainter]
		protected IPlayerKeybindsData keybinds;
		[FMODUnity.EventRef] [SerializeField] private string landEvent;
		private StateForMovement previousState;
		private RopeSystem rope;
		private PlayerSwinging pSwinging;
		//private float startPos;
		//private float endPos;
		//[SerializeField] private float dmgThreshold = 7f;
		//[SerializeField] private int startingDmg = 5;
		//[SerializeField] private float deathHeight = 20f;
		//private float dmgFactor;
		//CharacterTakeDamage charDmg;

		protected override void Initialization_State()
		{
			base.Initialization_State();
			Priority = 10;
			keybinds = SaveAndLoadData<IPlayerKeybindsData>.LoadSpecificData("Keybinds");
			rope = GetComponentInChildren<RopeSystem>();
			pSwinging = GetComponent<PlayerSwinging>();
			//charDmg = GetComponent<CharacterTakeDamage>();
			//startPos = transform.position.y;
			//endPos = startPos;
			//dmgFactor = (GetComponent<CharacterStatsMono>().MaxHealth - startingDmg) / (deathHeight - dmgThreshold);
		}

		public override void OnEnter_State()
		{
			base.OnEnter_State();
			//check if already swinging => detach
			//else if rope attached but not swinging => jump & at highest point start swinging
			//if (previousState is PlayerSwinging || previousState is PlayerSwingingIdle)
			if (pSwinging.IsSwinging)
			{
				rope.DetachRope();
			}
			
			rigBody.velocity = new Vector2(rigBody.velocity.x, MovementData.Gravity * MovementData.JumpHeightMultiplicator);
		}

		public override void Update_State()
		{
			MovementData.HorizontalMovement = (Input.GetKey(keybinds.KeyboardRight) ? 1 : 0) + (Input.GetKey(keybinds.KeyboardLeft) ? -1 : 0);
			previousState = controller.ActiveStateMovement;
			//if (PlayerGravity.CollisionCount != 0 && controller.ActiveStateMovement != this && Input.GetKeyDown(keybinds.KeyboardJump))
			if (PlayerGravity.IsGrounded && controller.ActiveStateMovement != this && Input.GetKeyDown(keybinds.KeyboardJump) 
				&& (!rope.RopeAttached || rope.Anchor == General.Enums.AnchorType.Swing))
			{
				controller.SwapState(this);
			}

			//if (!previousGroundedStatus && PlayerGravity.IsGrounded)
			//{
			//	Debug.Log("Velocity: " + rigBody.velocity.y);
			//}
			//previousGroundedStatus = PlayerGravity.IsGrounded;
		}

		public override void WhileActive_State()
		{
			if (MovementData.HorizontalMovement != 0)
			{
				rigBody.gameObject.transform.localScale = new Vector3(MovementData.HorizontalMovement, 1, 1);
			}

			rigBody.velocity = new Vector2(MovementData.MovementSpeed * MovementData.HorizontalMovement, rigBody.velocity.y);
			if (rigBody.velocity.y <= 0 && rope.RopeAttached && rope.Anchor == General.Enums.AnchorType.Swing)
			{
				controller.ForceSwapState(pSwinging);
			}
		}

		public void OnTriggerExit2D(Collider2D collision)
		{
			//if (collision.gameObject.tag == PlayerGravity.ObjectsThatEnableJump)
			//	//PlayerGravity.CollisionCount--;
			//	PlayerGravity.IsGrounded = false;
			PlayerGravity.GroundedTrigger(collision.tag, false);
			//if (PlayerGravity.TagIsGroundedTrigger(collision.tag))
			//{
			//	startPos = transform.position.y;
			//}
		}

		public void OnTriggerEnter2D(Collider2D collision)
		{
			//if (collision.gameObject.tag == PlayerGravity.ObjectsThatEnableJump)
			//	//PlayerGravity.CollisionCount++;
			//	PlayerGravity.IsGrounded = true;
			PlayerGravity.GroundedTrigger(collision.tag, true);

			//if (PlayerGravity.CollisionCount > 0)
			if (PlayerGravity.IsGrounded)
			{
				GetComponentInChildren<SoundControllerHelper>().PlaySound(landEvent);
				controller.EndState(this);
			}

			//if (PlayerGravity.TagIsGroundedTrigger(collision.tag))
			//{
			//	endPos = transform.position.y;
			//	float diffPos = Mathf.Abs(endPos - startPos);
			//	if (diffPos > dmgThreshold)
			//	{
			//		int dmg = 20;
			//		dmg = Mathf.RoundToInt((diffPos - dmgThreshold) * dmgFactor + startingDmg);
			//		charDmg.TakeDamage(dmg);
			//		Debug.Log("Diff in pos: " + diffPos);
			//		Debug.Log("dmg: " + dmg);
			//	}
			//}
		}

		//private void OnCollisionEnter2D(Collision2D collision)
		//{
		//	if (PlayerGravity.TagIsGroundedTrigger(collision.collider.tag))
		//	{
		//		Debug.Log("Vel: " + collision.relativeVelocity.magnitude);
		//		endPos = transform.position.y;
		//		Debug.Log("Diff in pos: " + Mathf.Abs(endPos - startPos));

		//	}

		//}

		//private void OnCollisionExit2D(Collision2D collision)
		//{
		//	if (PlayerGravity.TagIsGroundedTrigger(collision.collider.tag))
		//	{
		//		startPos = transform.position.y;
		//	}
		//}

		public void OnTriggerStay2D(Collider2D collision)
		{
			if (!PlayerGravity.IsGrounded)
			{
				PlayerGravity.GroundedTrigger(collision.tag, true);
			}
		}
	}
}
