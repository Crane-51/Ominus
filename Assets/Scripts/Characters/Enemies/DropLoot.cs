using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General.State;
using Data.Items;

[System.Serializable]
public class Loot
{
	public Item item;
	[Range(0.0f, 1.0f)] public float dropRate;
}

public class DropLoot : MonoBehaviour
{
	[SerializeField] private bool canDrop = false;
	[SerializeField] private List<Loot> loots;

	private void Start()
	{
		float sum = 0f;
		for (int i = 0; i < loots.Count; i++)
		{
			sum += loots[i].dropRate;
		}

		if (sum > 1)
		{
			Debug.LogError("Sum of drop rates is bigger than 1");
		}
	}

	public void Drop()
	{
		if (canDrop)
		{
			float rand = Random.value;
			float min = 0f;
			Item itemToDrop = null;
			foreach (Loot loot in loots)
			{
				if (rand > min && rand <= loot.dropRate)
				{
					itemToDrop = loot.item;
					//Debug.Log("Loot: " + loot.item + " " + loot.dropRate);
					break;
				}
				min = loot.dropRate; // test this whole foreach
			}
			if (itemToDrop != null)
			{
				if (itemToDrop is PuzzleItem)
				{
					itemToDrop.transform.position = this.transform.position;
					itemToDrop.gameObject.SetActive(true);
				}
				else
				{
					Instantiate(itemToDrop, transform.position, transform.rotation);
				}
			}
			else
			{
				Debug.Log("Loot: nothing");
			}

		}
	}

}
