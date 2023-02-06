using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class AutoScroll : MonoBehaviour
{
	//[SerializeField] bool debug;
	//[SerializeField] ScrollRect scrollRect;
	//[SerializeField] Scrollbar scrollbar;
	//[SerializeField] float scrollPadding = 20f;

	//private void OnEnable()
	//{
	//	StartCoroutine(DetectScroll());
	//}

	//IEnumerator DetectScroll()
	//{
	//	GameObject current;
	//	GameObject prevGo = null;
	//	Rect currentRect = new Rect();
	//	Rect viewRect = new Rect();
	//	RectTransform view = scrollRect.GetComponent<RectTransform>();

	//	while (true)
	//	{
	//		current = EventSystem.current.currentSelectedGameObject;
	//		if (current != null /*&& current.transform.parent == transform*/)
	//		{
	//			// Get a cached instance of the RectTransform
	//			if (current != prevGo)
	//			{
	//				RectTransform rt = current.GetComponent<RectTransform>();

	//				// Create rectangles for comparison
	//				currentRect = GetRect(current.transform.position, rt.rect, Vector2.zero);
	//				viewRect = GetRect(scrollRect.transform.position, view.rect, view.offsetMax);
	//				Vector2 heading = currentRect.center - viewRect.center;

	//				if (heading.y > 0f && !viewRect.Contains(currentRect.max))
	//				{
	//					float distance = Mathf.Abs(currentRect.max.y - viewRect.max.y) + scrollPadding;
	//					view.anchoredPosition = new Vector2(view.anchoredPosition.x, view.anchoredPosition.y - distance);
	//					if (debug) Debug.LogFormat("Scroll up {0}", distance); // Decrease y value
	//				}
	//				else if (heading.y < 0f && !viewRect.Contains(currentRect.min))
	//				{
	//					float distance = Mathf.Abs(currentRect.min.y - viewRect.min.y) + scrollPadding;
	//					view.anchoredPosition = new Vector2(view.anchoredPosition.x, view.anchoredPosition.y + distance);
	//					if (debug) Debug.LogFormat("Scroll down {0}", distance); // Increase y value
	//				}

	//				// Get adjusted rectangle positions
	//				currentRect = GetRect(current.transform.position, rt.rect, Vector2.zero);
	//				viewRect = GetRect(scrollRect.transform.position, view.rect, view.offsetMax);
	//			}
	//		}

	//		prevGo = current;

	//		if (debug)
	//		{
	//			DrawBoundary(viewRect, Color.cyan);
	//			DrawBoundary(currentRect, Color.green);
	//		}

	//		yield return null;
	//	}
	//}

	//static Rect GetRect(Vector3 pos, Rect rect, Vector2 offset)
	//{
	//	float x = pos.x + rect.xMin - offset.x;
	//	float y = pos.y + rect.yMin - offset.y;
	//	Vector2 xy = new Vector2(x, y);

	//	return new Rect(xy, rect.size);
	//}

	//public static void DrawBoundary(Rect rect, Color color)
	//{
	//	Vector2 topLeft = new Vector2(rect.xMin, rect.yMax);
	//	Vector2 bottomRight = new Vector2(rect.xMax, rect.yMin);

	//	Debug.DrawLine(rect.min, topLeft, color); // Top
	//	Debug.DrawLine(rect.max, topLeft, color); // Left
	//	Debug.DrawLine(rect.min, bottomRight, color); // Bottom
	//	Debug.DrawLine(rect.max, bottomRight, color); // Right
	//}

	RectTransform scrollRectTransform;
	RectTransform contentPanel;
	RectTransform viewportPanel;
	RectTransform selectedRectTransform;
	RectTransform lastSelectedRectTransform;
	GameObject lastSelected;
	float lastY = 0f;
	float scrollViewMinY;
	float scrollViewMaxY;
	Vector2 startingPosition;
	bool firstRun = true;
	Scrollbar scrollbar;
	float layoutSpacing;

	void OnEnable()
	{
		scrollRectTransform = GetComponent<RectTransform>();
		contentPanel = GetComponent<ScrollRect>().content;
		viewportPanel = GetComponent<ScrollRect>().viewport;
		scrollbar = GetComponent<ScrollRect>().verticalScrollbar;
		layoutSpacing = contentPanel.GetComponent<VerticalLayoutGroup>().spacing;
		if (firstRun)
		{
			Debug.Log(contentPanel.anchoredPosition);
			startingPosition = contentPanel.anchoredPosition;
			firstRun = false;
		}
		else
		{
			contentPanel.anchoredPosition = startingPosition;
			lastY = contentPanel.anchoredPosition.y;
		}
		// The upper bound of the scroll view is the anchor position of the content we're scrolling.
		scrollViewMinY = viewportPanel.anchoredPosition.y;
		// The lower bound is the anchor position + the height of the scroll rect.
		scrollViewMaxY = viewportPanel.anchoredPosition.y + scrollRectTransform.rect.height;
	}

	void Update()
	{
		// Get the currently selected UI element from the event system.
		GameObject selected = EventSystem.current.currentSelectedGameObject;

		// Return if there are none.
		if (selected == null)
		{
			return;
		}
		// Return if the selected game object is not inside the scroll rect.
		if (selected.transform.parent != contentPanel.transform)
		{
			return;
		}
		// Return if the selected game object is the same as it was last frame,
		// meaning we haven't moved.
		if (selected == lastSelected)
		{
			//if (lastY != contentPanel.anchoredPosition.y)
			//{
			//	lastY = contentPanel.anchoredPosition.y;
			//}
			return;
		}

		// Get the rect tranform for the selected game object.
		selectedRectTransform = selected.GetComponent<RectTransform>();
		if (lastSelected != null)
		{
			lastSelectedRectTransform = lastSelected.GetComponent<RectTransform>(); 
		}
		else
		{
			lastSelectedRectTransform = selectedRectTransform;
		}
		// The position of the selected UI element is the absolute anchor position,
		// ie. the local position within the scroll rect + its height if we're
		// scrolling down. If we're scrolling up it's just the absolute anchor position.
		float selectedPositionY = Mathf.Abs(selectedRectTransform.anchoredPosition.y) + selectedRectTransform.rect.height + layoutSpacing;
		Debug.Log("selected anchor: " + Mathf.Abs(selectedRectTransform.anchoredPosition.y) + " Height: " + selectedRectTransform.rect.height + " SelectedPosY: " + selectedPositionY);
		// The upper bound of the scroll view is the anchor position of the content we're scrolling.
		//float scrollViewMinY = viewportPanel.anchoredPosition.y;
		//// The lower bound is the anchor position + the height of the scroll rect.
		//float scrollViewMaxY = viewportPanel.anchoredPosition.y + scrollRectTransform.rect.height;
		Debug.Log("scroll min: " + scrollViewMinY + " scroll max: " + scrollViewMaxY);
		Debug.Log("prev: " + lastY + " curr: " + contentPanel.anchoredPosition.y + " diff: " + (contentPanel.anchoredPosition.y - lastY));
		float diff = (contentPanel.anchoredPosition.y - lastY);
		Debug.Log("scroll min + diff: " + (scrollViewMinY + diff) + " scroll max + diff: " + (scrollViewMaxY + diff));
		scrollViewMaxY += diff;
		scrollViewMinY += diff;
		lastY = contentPanel.anchoredPosition.y;
		// If the selected position is below the current lower bound of the scroll view we scroll down.
		if (selectedPositionY > scrollViewMaxY && scrollbar.value > 0)
		{
			float newY = selectedPositionY - scrollRectTransform.rect.height;
			//contentPanel.anchoredPosition = new Vector2(contentPanel.anchoredPosition.x, newY);
			//contentPanel.anchoredPosition = new Vector2(contentPanel.anchoredPosition.x, contentPanel.anchoredPosition.y + selectedRectTransform.rect.height);
			contentPanel.anchoredPosition = new Vector2(contentPanel.anchoredPosition.x, contentPanel.anchoredPosition.y + lastSelectedRectTransform.rect.height + layoutSpacing);
			Debug.Log("Scroll down: " + contentPanel.anchoredPosition.y);
		}
		// If the selected position is above the current upper bound of the scroll view we scroll up.
		//else if (Mathf.Abs(selectedRectTransform.anchoredPosition.y) > scrollViewMaxY)
		else if (Mathf.Abs(selectedRectTransform.anchoredPosition.y) + layoutSpacing - selectedRectTransform.rect.height * selectedRectTransform.pivot.y < scrollViewMinY && scrollbar.value < 1)
		{
			//contentPanel.anchoredPosition = new Vector2(contentPanel.anchoredPosition.x, contentPanel.anchoredPosition.y - lastSelectedRectTransform.rect.height);
			contentPanel.anchoredPosition = new Vector2(contentPanel.anchoredPosition.x, contentPanel.anchoredPosition.y - selectedRectTransform.rect.height - layoutSpacing);
			//contentPanel.anchoredPosition = new Vector2(contentPanel.anchoredPosition.x, Mathf.Abs(selectedRectTransform.anchoredPosition.y) - selectedRectTransform.rect.height);
			Debug.Log("Scroll up: " + contentPanel.anchoredPosition.y);
		}
		else
		{
			Debug.Log("Scrolls not satisfied");
		}		

		lastSelected = selected;
		
	}
}
