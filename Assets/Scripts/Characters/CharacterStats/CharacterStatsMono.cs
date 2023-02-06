using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using General.Enums;
using System.Collections.Generic;
using Data.Items;
using System.Linq;
using UnityEngine;

namespace Character.Stats
{
    public class CharacterStatsMono : HighPriorityState
    {
        /// <summary>
        /// Gets or sets character stats.
        /// </summary>
        [InjectDiContainter]
        public IStats CharacterStats { get; set; }

        public bool IsCharacterStatsAvaliable { get { return CharacterStats != null; } }

		public int Strength { get { return CharacterStats.Strength; } }
		public int Dexterity { get { return CharacterStats.Dexterity; } }

		public WeaponId Weapon { get { return CharacterStats.Weapon; } }

        public int CurrentHealth { get { return this.CharacterStats.CurrentHealth; } }
        public int MaxHealth { get { return this.CharacterStats.MaxHealth; } }

        public int CurrentMana { get { return this.CharacterStats.CurrentMana; } }
        public int MaxMana { get { return this.CharacterStats.MaxMana; } }

        public int CurrentImagination { get { return this.CharacterStats.CurrentImagination; } }
        public int MaxImagination { get { return this.CharacterStats.MaxImagination; } }

		//public Equipment EquippedItem { get; set; }

		private List<string> idWithHealthbar = new List<string> { "Monk", "SkeletonArcher", "SkeletonSwordman" };
		//private Equipment defaultItem;
		//private GameObject crosshair;

		public void TakeDamage(int damage)
        {
            CharacterStats.RemoveHealth(damage, controller.Id);

			if (idWithHealthbar.Contains(controller.Id))
			{
				EnemyHealthBar ehb = GetComponent<EnemySharedDataAndInit>().HealthBar;
				if (ehb != null)
				{
					ehb.ChangeHealth((float)CurrentHealth / MaxHealth); 
				}
			}
		}

        public void AddHealth(int healthAmount)
        {
            CharacterStats.AddHealth(healthAmount, controller.Id);
        }

        public void SetHealth(int healthToSet)
        {
            CharacterStats.CurrentHealth = healthToSet;
            //StatsController.AddStat(General.Enums.PlayerUIStatsForUpdate.Health, controller.Id);
        }

		//public void EquipDefault()
		//{
		//	if (defaultItem != null)
		//	{
		//		defaultItem.Equip(); 
		//	}
		//}

		//public void EquippedRanged()
		//{
		//	if (crosshair != null)
		//	{
		//		crosshair.GetComponent<SpriteRenderer>().enabled = true;
		//	}
		//}

		//public void UnequippedRanged()
		//{
		//	if (crosshair != null)
		//	{
		//		crosshair.GetComponent<SpriteRenderer>().enabled = false;
		//	}
		//}

		private void Awake()
		{
			//CharacterStats = SaveAndLoadData<IStats>.LoadSpecificData(GetComponent<StateController>().Id);
			CharacterStats = new Implementation.Data.Stats();
		}

		protected override void Initialization_State()
        {
            base.Initialization_State();
            CharacterStats = SaveAndLoadData<IStats>.LoadSpecificData(controller.Id);
			if (controller.Id == "Player")
			{
				Implementation.Data.Runtime.CheckpointController.singleton.LoadCheckpoint();
				//defaultItem = GetComponentsInChildren<Equipment>().ToList().FirstOrDefault(x => x.name == "Fists");
				//if (EquippedItem == null)
				//{
				//	//equip fists
				//	//Equipment fists = GetComponentsInChildren<Equipment>().ToList().FirstOrDefault(x => x.name == "Fists");
				//	if (defaultItem != null)
				//	{
				//		defaultItem.Equip();
				//	}
				//}
				
				//foreach (var child in GetComponentsInChildren<Transform>())
				//{
				//	if (child.name == "Crosshair")
				//	{
				//		crosshair = child.gameObject;
				//		Debug.Log("Found crosshair");
				//		break;
				//	}
				//}
			}
        }
    }
}
