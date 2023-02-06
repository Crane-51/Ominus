using UnityEditor;
using Implementation.Data;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(EnemyData))]
public class EnemyDataDrawerUIE : SeedDataDrawer
{
	protected override void SetElementLists()
	{
		listOfProperties = new List<string>() { "rangeOfVision", "angleOfVision", "minRangeOfAttack", "maxRangeOfAttack", "canAttack", "attackCooldown" };
		listOfLabels = new List<string>() { "Range Of Vision", "Angle Of Vision", "Min Range Of Attack", "Max Range Of Attack", "Can Attack?", "Attack Cooldown" };
	}
}
