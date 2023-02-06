using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
public class UIAnimationControl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    private Animator anima { get; set; }

    private const string HighlightedAnimation = "Highlighted";

    public void OnPointerEnter(PointerEventData eventData)
    {
        anima.SetBool(HighlightedAnimation, true);
    }

    // Use this for initialization
    void Awake () {
        anima = GetComponent<Animator>();	
	}

    public void OnPointerExit(PointerEventData eventData)
    {
        anima.SetBool(HighlightedAnimation, false);
    }
}
