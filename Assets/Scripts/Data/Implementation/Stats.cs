using System;
using Character.Stats.UI;
using General.Enums;

namespace Implementation.Data
{
    [Serializable]
    public class Stats : CharacterStats, IStats
    {
        /// <inheritdoc />
        public int CurrentMana { get; set; }
		public int currentMana;

        /// <inheritdoc />
        public int MaxMana { get; set; }
		public int maxMana;

        /// <inheritdoc />
        public int CurrentImagination { get; set; }
		public int currentImagination;

        /// <inheritdoc />
        public int MaxImagination { get; set; }
		public int maxImagination;

		/// <inheritdoc />
		public WeaponId Weapon { get { return weapon; } set { weapon = value; } }
		public WeaponId weapon;

        /// <inheritdoc />
        public void AddImagination(int imagination, string id)
        {
            StatsController.AddStat(PlayerUIStatsForUpdate.Imagination, id);
            CurrentImagination += imagination;

            if(CurrentImagination > MaxImagination)
            {
                CurrentImagination = MaxImagination;
            }

        }

        /// <inheritdoc />
        public void AddMana(int mana, string id)
        {
            CurrentMana += mana;

            if (CurrentMana > MaxMana)
            {
                CurrentMana = MaxMana;
            }
            StatsController.AddStat(PlayerUIStatsForUpdate.Mana, id);
        }

        /// <inheritdoc />
        public bool RemoveImagination(int imagination, string id)
        {
            if(CurrentImagination-imagination >= 0)
            {
                CurrentImagination -= imagination;
                StatsController.AddStat(PlayerUIStatsForUpdate.Imagination, id);
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public bool RemoveMana(int mana, string id)
        {
            if (CurrentMana - mana >= 0)
            {
                CurrentMana -= mana;
                StatsController.AddStat(PlayerUIStatsForUpdate.Mana, id);
                return true;
            }

            return false;
        }
    }
}
