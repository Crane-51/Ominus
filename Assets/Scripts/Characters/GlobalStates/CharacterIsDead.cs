using General.State;
using UnityEngine;

namespace Character.Stats
{
    public class CharacterIsDead : HighPriorityState
    {
        /// <summary>
        /// Gets or sets rigid body of player character.
        /// </summary>
        private Rigidbody2D rigb { get; set; }

		private ParticleSystem ps { get; set; }
        [SerializeField] private ParticleSystem deathParticle = null;

        /// <summary>
        /// Gets or sets related informations.
        /// </summary>
        private CharacterStatsMono CharacterStats { get; set; }

		private EnemySharedDataAndInit enemyStats;
		protected EquipmentManager equipManager;

		protected override void Initialization_State()
        {
            base.Initialization_State();
            Priority = int.MaxValue;
            rigb = GetComponent<Rigidbody2D>();
			ps = GetComponentInChildren<ParticleSystem>();
            CharacterStats = GetComponent<CharacterStatsMono>();
			enemyStats = GetComponent<EnemySharedDataAndInit>();
			if (tag == "Player")
			{
				equipManager = GetComponent<EquipmentManager>(); 
			}
		}

        public override void OnEnter_State()
        {
            base.OnEnter_State();
			if (tag != "Player")
			{
				Collider2D collider = GetComponent<CapsuleCollider2D>();
				if (collider == null)
				{
					collider = GetComponent<BoxCollider2D>();
				}
				Destroy(collider); 
			}
			if (ps != null)
			{
                Destroy(ps.gameObject);
            }
            if (deathParticle)
            {
                Instantiate(deathParticle, transform.position, transform.rotation);

            }
			if (enemyStats != null)
			{
				enemyStats.HealthBar.DestroySlider();
			}
			if (GetComponent<DropLoot>() != null)
			{
				GetComponent<DropLoot>().Drop();
			}

			if (equipManager != null && equipManager.EquippedItem != null && equipManager.EquippedItem.name != "Fists")
			{
				equipManager.EquippedItem.Unequip();
			}
		}

        public override void Update_State()
        {
            if (controller.ActiveHighPriorityState != this && CharacterStats.CurrentHealth <= 0)
            {
                controller.SwapState(this);
            }
        }

        public override void WhileActive_State()
        {
            base.WhileActive_State();

            if(rigb.velocity.y > -0.1f && rigb.velocity.y < 0.1f)
            {
                rigb.bodyType = RigidbodyType2D.Static;
            }
            else
            {
				Debug.Log(rigb.velocity);
                rigb.velocity = new Vector2(0, rigb.velocity.y);
            }
        }
    }
}
