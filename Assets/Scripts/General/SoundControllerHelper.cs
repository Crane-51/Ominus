using UnityEngine;

public class SoundControllerHelper : MonoBehaviour
{
	public void PlaySound(string path)
	{
		FMOD.Studio.EventInstance eventInstance;
		eventInstance = FMODUnity.RuntimeManager.CreateInstance(path);
		eventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
		eventInstance.start();
		eventInstance.release();
	}

	public void PlaySound(AudioClip clip)
	{
		AudioSource src = GetComponent<AudioSource>();
		if (src != null)
		{			
			src.PlayOneShot(clip);
		}
	}
}
