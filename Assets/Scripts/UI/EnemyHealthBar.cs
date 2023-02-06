using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
	[HideInInspector] public Transform objectToFollow;
	public Vector3 localOffset;

	void FixedUpdate()
	{
		if (objectToFollow != null)
		{
			transform.position = Camera.main.WorldToScreenPoint(objectToFollow.position + localOffset);
		}
	}

	public void ChangeHealth(float percentage)
	{
		GetComponent<Slider>().value = percentage;
	}

	public void DestroySlider()
	{
		Destroy(gameObject);
	}
}
