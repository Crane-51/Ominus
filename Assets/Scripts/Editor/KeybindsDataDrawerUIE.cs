using UnityEditor;
using Implementation.Data;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(PlayerKeybindsData))]
public class KeybindsDataDrawerUIE : SeedDataDrawer
{
	protected override void SetElementLists()
	{
		listOfProperties = new List<string>() { "keyboardLeft", "keyboardRight", "keyboardUp", "keyboardDown", "keyboardJump", "keyboardUse",
			"keyboardPunchKey", "keyboardStealthKey", "keyboardCrouchKey", "keyboardInventory", "keyboardDodge" };
		listOfLabels = new List<string>() { "Left", "Right", "Up", "Down", "Jump", "Use", "Punch", "Stealth", "Crouch", "Inventory", "Dodge"};
	}
}
