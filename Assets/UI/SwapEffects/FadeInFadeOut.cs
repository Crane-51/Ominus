using System.Collections;
using DiContainerLibrary.DiContainer;
using General.Enums;
using Implementation.Data;
using UnityEngine;
using UnityEngine.UI;

public class FadeInFadeOut : SwapEffects
{
    /// <summary>
    /// Gets or sets game information data.
    /// </summary>
    [InjectDiContainter]
    private IGameInformation gameInformation { get; set; }

    /// <summary>
    /// Gets or sets image for fading.
    /// </summary>
    public Image ImageForEffect;

    /// <summary>
    /// Gets or sets fade speed.
    /// </summary>
    public float FadeSpeed;

    /// <summary>
    /// Gets or sets fade effects.
    /// </summary>
    public EffectDirection FadeDirection;

    /// <summary>
    /// Gets or sets background music.
    /// </summary>
    public AudioSource BackgroundMusic;

    [Range(0,1)]
    public float BackgroundMusicVolume;

    /// <summary>
    /// Created image position.
    /// </summary>
    protected Image CreatedImage { get; set; }

    public override void OnActivation(OnFinishLoad action, EffectDirection effectDirection, float fadeSpeed = 0.5f)
    {
        FadeSpeed = fadeSpeed;
        if (effectDirection == EffectDirection.FadeOut)
        {
            ImageForEffect.GetComponent<Image>().color = new Color(0, 0, 0, 0);

            if (BackgroundMusic != null)
            {
                BackgroundMusic.volume = BackgroundMusicVolume;
            }
        }
        else if (effectDirection == EffectDirection.FadeIn)
        {
            ImageForEffect.GetComponent<Image>().color = new Color(0, 0, 0, 1);

            if (BackgroundMusic != null)
            {
                BackgroundMusic.volume = 0;
            }
        }
        else
        {
            return;
        }

        FadeDirection = effectDirection;
        onFinishLoad = action;
        CreatedImage = Instantiate(ImageForEffect, transform);
    }

    protected override void OnStart()
    {
        gameInformation.StopMovement = true;
        OnActivation(null, FadeDirection);
    }

    protected override void OnUpdate()
    {
        var speed = FadeSpeed * Time.unscaledDeltaTime;
        if (CreatedImage != null && FadeDirection == EffectDirection.FadeOut)
        {
            CreatedImage.color = new Color(0, 0, 0, CreatedImage.color.a + speed);

            if (BackgroundMusic != null)
            {
                BackgroundMusic.volume -= speed;
            }

            if (CreatedImage.color.a > 1)
            {
                OnEffectFinished();
            }
        }
        else if (CreatedImage != null && FadeDirection == EffectDirection.FadeIn)
        {
            CreatedImage.color = new Color(0, 0, 0, CreatedImage.color.a - speed);

            if (BackgroundMusic != null && BackgroundMusicVolume > BackgroundMusic.volume)
            {
                BackgroundMusic.volume += speed;
            }

            if (CreatedImage.color.a < 0)
            {
                OnEffectFinished();
            }
        }
    }

    protected override void OnEffectFinished()
    {
        if (onFinishLoad != null)
        {
            onFinishLoad();
        }

        if (FadeDirection == EffectDirection.FadeIn)
        {
            Destroy(CreatedImage.gameObject);
            CreatedImage = null;
        }
        else if (FadeDirection == EffectDirection.FadeOut)
        {
            CreatedImage = null;
        }

        gameInformation.StopMovement = false;
    }
}
