using UnityEditor;
using Implementation.Data;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(WeaponData))]
public class WeaponDataDrawerUIE : SeedDataDrawer
{
	protected override void SetElementLists()
	{
		listOfProperties = new List<string>() { "dieToRoll", "type" };
		listOfLabels = new List<string>() { "Die To Roll", "Weapon Type" };
	}
}
