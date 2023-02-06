using System;
using Character.Stats.UI;

namespace Implementation.Data
{
    [Serializable]
    public class CharacterStats : ICharacterStats
    {
        /// <inheritdoc />
        public string Id { get; set; }
	//	public string id;

  //      /// <inheritdoc />
  //      public string Name { get; set; }
		//public string name;

        /// <inheritdoc />
        public int CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }
		public int currentHealth;

        /// <inheritdoc />
        public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
		public int maxHealth;

		/// <inheritdoc />
		public int Strength { get { return strength; } set { strength = value; } }
		public int strength;

		/// <inheritdoc />
		public int Dexterity { get { return dexterity; } set { dexterity = value; } }
		public int dexterity;

		

		/// <inheritdoc />
		public void AddHealth(int health, string id)
        {
            StatsController.AddStat(General.Enums.PlayerUIStatsForUpdate.Health, id);
            CurrentHealth += health;

            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
        }

        /// <inheritdoc />
        public bool RemoveHealth(int health, string id)
        {
            StatsController.AddStat(General.Enums.PlayerUIStatsForUpdate.Health, id);
            CurrentHealth -= health;


            if (CurrentHealth <= 0)
            {
                return true;
            }
            return false;
        }

		public CharacterStats()
		{
			MaxHealth = 10;
			CurrentHealth = 7;
		}
	}
}
