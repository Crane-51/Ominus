using UnityEditor;
using Implementation.Data;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(MovementData))]
public class MovementDataDrawerUIE : SeedDataDrawer
{
	protected override void SetElementLists()
	{
		listOfProperties = new List<string>() { "movementSpeed", "jumpHeightMultiplicator", "gravity", "gravityEqualizator" };
		listOfLabels = new List<string>() { "Movement Speed", "Jump Height Multiplicator", "Gravity", "Gravity Equalizator" };
	}
}