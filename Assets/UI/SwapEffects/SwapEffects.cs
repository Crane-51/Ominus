using General.Enums;
using UnityEngine;

public abstract class SwapEffects : MonoBehaviour
{
    public static SwapEffects singleton { get; set; }

    public delegate void OnFinishLoad();

    public OnFinishLoad onFinishLoad { get; set; }

    public abstract void OnActivation(OnFinishLoad action, EffectDirection effectDirection, float fadeSpeed = 0.5f);

    protected abstract void OnStart();

    protected abstract void OnUpdate();

    protected abstract void OnEffectFinished();


    private void Start()
    {
        singleton = this;
        DiContainerLibrary.DiContainer.DiContainerInitializor.RegisterObject(this);
        OnStart();
    }

    private void Update()
    {
        OnUpdate();
    }
}
