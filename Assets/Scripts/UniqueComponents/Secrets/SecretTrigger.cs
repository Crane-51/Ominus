using System.Collections;
using General.State;
using Secret;
using UnityEngine;

namespace UniqueComponent.Secret
{
	public class SecretTrigger : HighPriorityState
	{
		public const float waitBeforeDestroy = 0.5f;
		/// <summary>
		/// Gets or sets value indicating if secret was found or not.
		/// </summary>
		private bool IsFound { get; set; }

		/// <summary>
		/// Gets or sets renderer component.
		/// </summary>
		private SpriteRenderer Renderer { get; set; }

		protected override void Initialization_State()
		{
			base.Initialization_State();
			this.Renderer = GetComponent<SpriteRenderer>();
		}

		public override void OnEnter_State()
		{
			base.OnEnter_State();
			SecretCounter.SecretFound(this.gameObject.name);
			IsFound = true;
			StartCoroutine(destroyAfter());
		}

		public void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.tag == "Player" && !IsFound)
			{
				controller.SwapState(this);
			}
		}

		private IEnumerator destroyAfter()
		{
			yield return new WaitUntil(() => designController.animationController.IsAnimationOver(this));
			//yield return new WaitForSeconds(waitBeforeDestroy);
			Destroy(this.gameObject);
		}
	}
}