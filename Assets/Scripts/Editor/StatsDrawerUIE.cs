using UnityEditor;
using Implementation.Data;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(Stats))]
public class StatsDrawerUIE : SeedDataDrawer
{
	protected override void SetElementLists()
	{
		listOfProperties = new List<string>() { "currentHealth", "maxHealth", "strength", "dexterity", "weapon" };//, "currentMana", "maxMana", "currentImagination", "maxImagination"};
		listOfLabels = new List<string>() { "Current Health", "Max Health", "Strength", "Dexterity", "Weapon" };//, "Current Mana", "Max Mana", "Current Imagination", "Max Imagination" };
	}

}
