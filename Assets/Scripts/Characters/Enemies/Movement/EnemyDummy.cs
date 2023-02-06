using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDummy : MonoBehaviour
{
	private CapsuleCollider2D capsuleCollider;
	private Rigidbody2D rbody;

    // Start is called before the first frame update
    void Start()
    {
		capsuleCollider = GetComponent<CapsuleCollider2D>();
		rbody = GetComponent<Rigidbody2D>();
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.tag == "Ground")
		{
			rbody.bodyType = RigidbodyType2D.Static;
			//capsuleCollider.enabled = false;
		}
	}
}
