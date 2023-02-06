using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class DialogBubbleSize : MonoBehaviour
{
	private RectTransform txtTransform;
	private RectTransform rectTransform;
	[SerializeField] private float heightPadding = 30f;
	private float previousHeight;
	private float rightX = 13f;
	private float leftX = -97.7f;
	[SerializeField] private int dir = 1;
	private int prevDir = 1;

    // Start is called before the first frame update
    void Start()
    {
		txtTransform = GetComponentInChildren<Text>().rectTransform;
		rectTransform = GetComponent<RectTransform>();
		previousHeight = txtTransform.rect.height;
	}

    // Update is called once per frame
    void Update()
    {
		if (previousHeight != txtTransform.rect.height)
		{
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, txtTransform.rect.height + heightPadding);
			previousHeight = txtTransform.rect.height;
		}

		if (prevDir != dir)
		{
			SwapX(dir);
			prevDir = dir;
		}
	}

	public void SwapX(int direction)
	{
		if (direction != 1 && direction != -1)
		{
			return;
		}

		if (rectTransform == null)
		{
			rectTransform = GetComponent<RectTransform>();
		}
		if (txtTransform == null)
		{
			txtTransform = GetComponentInChildren<Text>().rectTransform;
		}

		rectTransform.localScale = new Vector2(direction, 1);
		txtTransform.localScale = new Vector2(direction, 1);
		Vector2 tmp;
		switch (direction)
		{
			case 1:
			{
				tmp = new Vector2(0f, 0.5f);
				txtTransform.anchorMin = tmp;
				txtTransform.anchorMax = tmp;
				txtTransform.pivot = tmp;
				txtTransform.anchoredPosition = new Vector2(rightX, txtTransform.anchoredPosition.y);
				break;
			}
			case -1:
			{
				tmp = new Vector2(1f, 0.5f);
				txtTransform.anchorMin = tmp;
				txtTransform.anchorMax = tmp;
				txtTransform.pivot = tmp;
				txtTransform.anchoredPosition = new Vector2(leftX, txtTransform.anchoredPosition.y);
				break;
			}
		}
	}
}
