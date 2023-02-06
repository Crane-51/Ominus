using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using General.State;

public class HoveringStats : MonoBehaviour
{
	private class StatBubble
	{
		public Text txt;
		public Vector2 offset;
		public bool isStatic;
		public bool toBeDestroyed;
	}

	[SerializeField] private Transform canvas;
	[SerializeField] Text txtPrefab;
	private List<StatBubble> listOfStats = new List<StatBubble>();
	private StateController sc;
	private Text txtState;
	private Text txtAttack;
	private Text txtHealth;

	// Start is called before the first frame update
	void Start()
	{
		//canvas = GameObject.FindGameObjectWithTag("StatsCanvas").transform;
		//txtState = Instantiate(txtPrefab, canvas, false);
		//sc = GetComponent<StateController>();
		sc = GetComponent<StateController>();
		txtState = InstantiateNewTxt(new Vector2(0f, 1.5f));
		txtHealth = InstantiateNewTxt(new Vector2(0f, 2f), false, false, Color.green);
		txtHealth.fontStyle = FontStyle.Bold;
	}

	void Update()
	{
		txtState.text = sc.ToString();
		txtHealth.text = GetComponent<Character.Stats.CharacterStatsMono>().CurrentHealth.ToString();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		for (int i = 0; i < listOfStats.Count; i++)
		{
			if (!listOfStats[i].isStatic)
			{
				listOfStats[i].txt.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(listOfStats[i].offset.x, listOfStats[i].offset.y));
			}
		}
	}

	private IEnumerator WaitAndDestroy(StatBubble bubble)
	{
		yield return new WaitUntil(() =>bubble.txt.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
		listOfStats.Remove(bubble);
		Destroy(bubble.txt.gameObject);
	}

	public Text InstantiateNewTxt(Vector2 offset, bool isStatic = false, bool toBeDestroyed = false, Color? color = null)
	{
		Text txt = Instantiate(txtPrefab, canvas, false);
		txt.color = color ?? Color.white;
		StatBubble bubble = new StatBubble()
		{
			txt = txt,
			offset = offset,
			isStatic = isStatic,
			toBeDestroyed = toBeDestroyed
		};
		listOfStats.Add(bubble);
		if (isStatic)
		{
			txt.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(offset.x, offset.y));
		}
		if (toBeDestroyed)
		{
			txt.GetComponent<Animator>().Play("HoverStat");
			StartCoroutine(WaitAndDestroy(bubble));
		}

		return txt;
	}

	public void PrintAttack(int dmg)
	{
		txtAttack = InstantiateNewTxt(new Vector2(transform.localScale.x, 1f), true, true, Color.white);
		txtAttack.text = dmg.ToString();
	}
}
