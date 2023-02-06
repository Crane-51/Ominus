using System.Collections.Generic;
using General.State;
using UnityEngine;

public class StateSoundData : MonoBehaviour
{
    public State State;
    public List<AudioClip> AudioClips;
    public bool Loop;
    public float LoopOffset;
	[FMODUnity.EventRef]
	public string EventName;
}
