using Character.Stats;
using UnityEngine;

namespace Data.Items
{
    public class HealthPotion : Item
    {
		[SerializeField] private int amtOfHealthToRestore = 3;

        public override Item UseItem(Transform objectThatUsesItem)
        {
            var healthStat = objectThatUsesItem.GetComponent<CharacterStatsMono>();

            if(healthStat != null && healthStat.CurrentHealth < healthStat.MaxHealth)
            {
                healthStat.AddHealth(amtOfHealthToRestore);
				return this;
			}

			return null;
        }
    }
}
