using System.Collections;
using Character.Stats;
using General.State;
using UnityEngine;

public class SpikeUpAndDown : HighPriorityState
{
	[SerializeField] private float delayInFirstSpawn;
	[SerializeField] private float activeTime;
	[SerializeField] private float inactiveTime;
	[SerializeField] private int damage;
	[SerializeField] private bool alwaysOn = false;
	[SerializeField] private float immunityToSpikes = 0.4f;
	private bool spikeActive;

    protected override void Initialization_State()
    {
        base.Initialization_State();
        controller.SwapState(this);
        spikeActive = alwaysOn;
    }

    public override void OnEnter_State()
    {
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(delayInFirstSpawn);
		if (!alwaysOn)
		{
			while (true)
			{
				yield return new WaitForSeconds(inactiveTime);
				spikeActive = true;

				base.OnEnter_State();
				//yield return new WaitUntil(() => this.designController.animationController.IsAnimationOver(this));
				yield return new WaitForSeconds(activeTime);

				//if (!alwaysOn)
				//{
					spikeActive = false;
					base.OnExit_State();
				//}
			} 
		}
		else
		{
			yield return new WaitForSeconds(inactiveTime);
			spikeActive = true;
			base.OnEnter_State();
		}
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var takeDamage = collision.gameObject.GetComponent<CharacterTakeDamage>();
        if (spikeActive && takeDamage != null)
        {
            spikeActive = false;
            takeDamage.TakeDamage(damage, 0, 1, false, immunityToSpikes);
        }
    }
}
