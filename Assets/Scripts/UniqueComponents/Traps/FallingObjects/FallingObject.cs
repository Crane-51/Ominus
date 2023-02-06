using System.Collections;
using Character.Stats;
using General.State;
using UnityEngine;

namespace UniqueComponent.FallingObject
{
    public class FallingObject : StateForMovement
    {
        [SerializeField] private int damage;
        [SerializeField] private bool canPlayerTakeDamage { get; set; }
		[SerializeField] private float speed = 1f;
		[SerializeField] private AudioClip fallingClip;
		[SerializeField] private AudioClip landedClip;
		private bool triggered = false;

        protected override void Initialization_State()
        {
            base.Initialization_State();
            canPlayerTakeDamage = true;
        }

        public override void OnEnter_State()
        {
			if (!triggered && fallingClip != null)
			{
				GetComponentInChildren<SoundControllerHelper>().PlaySound(fallingClip);
			}
			triggered = true;
            designController.StartTask(this);
        }

        public override void Update_State()
        {
            if (controller.ActiveStateMovement != this)
            {
                rigBody.velocity = Vector2.zero;
				if (triggered)
				{
					controller.SwapState(this);
				}
            }
        }

        public override void WhileActive_State()
        {
            base.WhileActive_State();
            rigBody.velocity = new Vector2(0, rigBody.velocity.y - /*MovementData.MovementSpeed*/speed * 5 * Time.deltaTime);
        }

        public override void OnExit_State()
        {
            designController.StartTask(this);
        }

        public void IsActivated()
        {
            StartCoroutine(FallingTimer());
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                if (canPlayerTakeDamage)
                {
                    var playerTakeDamage = collision.gameObject.GetComponent<CharacterTakeDamage>();

                    if (playerTakeDamage != null)
                    {
                        playerTakeDamage.TakeDamage(damage);
                    }
                    canPlayerTakeDamage = false;
                }
				triggered = false;
				//GetComponentInChildren<SoundControllerHelper>().PlaySound(landedClip);
			}
            else if (collision.gameObject.tag == "Ground")
            {
                controller.EndState(this);
                canPlayerTakeDamage = false;
				triggered = false;
				GetComponentInChildren<SoundControllerHelper>().PlaySound(landedClip);
			}
        }

        private IEnumerator FallingTimer()
        {
            yield return new WaitUntil(SoundDoneCheck);
            controller.SwapState(this);
        }

        private bool SoundDoneCheck()
        {
            if(!designController.soundController.audioPlayers[0].isPlaying)
            {
                return true;
            }
            return false;
        }
    }
}
