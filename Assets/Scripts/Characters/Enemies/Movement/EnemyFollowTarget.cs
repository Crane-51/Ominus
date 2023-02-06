using System.Linq;
using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;

namespace Enemy.State
{
    public class EnemyFollowTarget : StateForMovement
    {
        /// <summary>
        /// Gets or sets enemy data.
        /// </summary>
        [InjectDiContainter]
        private IGameInformation gameInformation { get; set; }

		/// <summary>
		/// Gets or sets physics overlap.
		/// </summary>
		[InjectDiContainter]
        private IPhysicsOverlap physicsOverlap { get; set; }
		private EnemyInvestigateMovement enemyInvestigateMovement;
		private EnemySharedDataAndInit sharedData;

        protected override void Initialization_State()
        {
            base.Initialization_State();
            Priority = 7;
			enemyInvestigateMovement = GetComponent<EnemyInvestigateMovement>();
			sharedData = GetComponent<EnemySharedDataAndInit>();
		}

		public override void OnEnter_State()
		{
			base.OnEnter_State();
			sharedData.forceAim = false;
		}

		public override void WhileActive_State()
        {
            base.WhileActive_State();
			//maybe check if already in melee range?
			// TODO/CHECK: second part of condition?! If inside attack range, archer should attack, not investigate
            if (!sharedData.targetInRangeOfVision || (sharedData.weaponData.Type == General.Enums.WeaponType.Ranged && !IsOutsideMaxRange()))
            {
				controller.ForceSwapState(enemyInvestigateMovement);
                //controller.EndState(this);
            }
            else
            {
				//TODO: only environment or also border?
				RaycastHit2D wallHit =
					Physics2D.Raycast(transform.position, transform.localScale.x == -1 ? Vector3.left : Vector3.right, 1, LayerMask.GetMask("Environment", "Border"));
				//Debug.DrawRay(transform.position, (transform.localScale.x == -1 ? Vector3.left : Vector3.right) * 1f, Color.magenta);
				if (wallHit.collider)
				{
					controller.EndState(this);
				}
				else
				{
					rigBody.velocity = new Vector2(MovementData.MovementSpeed * sharedData.enemyData.LookAtTarget(transform, gameInformation.Player.transform), rigBody.velocity.y);
				}
			}
        }


        public override void Update_State()
        {
			//maybe check if already in melee range?
            if(controller.ActiveStateMovement != this && !(controller.ActiveHighPriorityState is Character.Stats.CharacterIsDead) && sharedData.targetLocked && sharedData.targetInRangeOfVision)
			{
				if (sharedData.weaponData.Type == General.Enums.WeaponType.Melee || sharedData.weaponData.Type == General.Enums.WeaponType.Ranged && IsOutsideMaxRange())
				{
					controller.SwapState(this); 
				}
            }
        }

		// If starts causing problems -> raycast
		private bool IsOutsideMaxRange()
		{
			return Vector2.Distance(gameInformation.Player.transform.position, transform.position) > sharedData.enemyData.MaxRangeOfAttack;
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			if (collision.gameObject.tag == "Border" && controller.ActiveStateMovement == this)
			{
				controller.EndState(this);
			}
		}
	}
}
