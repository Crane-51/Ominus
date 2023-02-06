using General.Enums;
using UnityEngine;

public class HookAnchor : MonoBehaviour
{
	public AnchorType type;
	private Rigidbody2D rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		rb.bodyType = type == AnchorType.Swing ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;
	}
}
