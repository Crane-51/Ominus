using UnityEngine;

public static class DiceRoller
{
	public static int RollDie(General.Enums.Die dieType)
	{
		return Random.Range(1, (int)dieType + 1);
	}

	public static int RollDieWithModifier(General.Enums.Die dieType, int modifier)
	{
		return Random.Range(1, (int)dieType + 1) + modifier;
	}
}
