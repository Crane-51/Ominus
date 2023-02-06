using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DiContainerLibrary.DiContainer;
using UnityEngine;

namespace General.State
{
    public class SoundController : MonoBehaviour
    {
        /// <summary>
        /// Gets or sets
        /// </summary>
        private List<StateSoundData> stateSoundDatas { get; set; }

        /// <summary>
        /// Gets or sets last played audio clip.
        /// </summary>
        private Dictionary<State,AudioClip> lastAudioClips { get; set; }

        /// <summary>
        /// Gets or sets specific state loop timer.
        /// </summary>
        private Dictionary<State, IEnumerator> specificStateSoundTimers { get; set; }

        /// <summary>
        /// Gets or sets active state sound data.
        /// </summary>
        private StateSoundData activeStateSoundData { get; set; }

        /// <summary>
        /// Gets or sets collection of audio players.
        /// </summary>
        public List<AudioSource> audioPlayers { get; private set; }

        /// <summary>
        /// Gets or sets value that iterate through collection of audio sources.
        /// </summary>
        private int sampleIndex { get; set; }

        /// <summary>
        /// Timer for the next sound.
        /// </summary>
        private IEnumerator NextSoundTimer { get; set; }

		private FMOD.Studio.EventInstance eventInstance;

		private void Awake()
        {
            audioPlayers = GetComponentsInChildren<AudioSource>().ToList();
            stateSoundDatas = GetComponentsInChildren<StateSoundData>().ToList();
            specificStateSoundTimers = new Dictionary<State, IEnumerator>();
            lastAudioClips = new Dictionary<State, AudioClip>();
        }

        private void Start()
        {
            DiContainerInitializor.RegisterObject(this);
            foreach (var item in stateSoundDatas)
            {
                lastAudioClips.Add(item.State, null);
                specificStateSoundTimers.Add(item.State, PlayAgain());
            }
        }

        /// <summary>
        /// Plays the sound with given key.
        /// </summary>
        /// <param name="key">Key of the audio clips from <see cref="ISoundData"/></param>
        public void PlaySound(State state)
        {
            var selectedSoundData = stateSoundDatas.FirstOrDefault(x => x.State == state);

            if (selectedSoundData == null) return;

			if (selectedSoundData.EventName == null || selectedSoundData.EventName == "")
			{
				if (audioPlayers.Count == 0
						|| selectedSoundData.AudioClips == null
						|| selectedSoundData.AudioClips.Count == 0)
				{
					activeStateSoundData = null;
					return;
				}
				activeStateSoundData = selectedSoundData;

				if (!activeStateSoundData.Loop && activeStateSoundData.AudioClips.Count > 0)
				{
					audioPlayers[0].clip = NextClip();
					audioPlayers[0].Play();
				}

				LoopPlay(); 
			}
			else
			{
				eventInstance = FMODUnity.RuntimeManager.CreateInstance(selectedSoundData.EventName);
				eventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
				if (selectedSoundData.Loop)
				{
					StartCoroutine(LoopPlayEvent(selectedSoundData.LoopOffset));
				}
				else
				{
					eventInstance.start();
					eventInstance.release();
				}
				//eventInstance.release();
			}
        }

        /// <summary>
        /// Stops the sound.
        /// </summary>
        public void StopSound(State state)
        {
            if (activeStateSoundData != null)
            {
                StopCoroutine(specificStateSoundTimers[activeStateSoundData.State]);
            }

			if (eventInstance.isValid())
			{
				eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				StopCoroutine(LoopPlayEvent(0));
				eventInstance.release();
			}
        }

        /// <summary>
        /// Gets next clip to play.
        /// </summary>
        /// <returns>Audio clip to play.</returns>
        private AudioClip NextClip()
        {
            if (lastAudioClips != null && activeStateSoundData.AudioClips.Count > 1)
            {
                lastAudioClips[activeStateSoundData.State] = activeStateSoundData.AudioClips.Where(x => x != lastAudioClips[activeStateSoundData.State]).ToList()[Random.Range(0, activeStateSoundData.AudioClips.Count - 1)];
            }
            else
            {
                lastAudioClips[activeStateSoundData.State] = activeStateSoundData.AudioClips[Random.Range(0, activeStateSoundData.AudioClips.Count)];
            }
            return lastAudioClips[activeStateSoundData.State];
        }

        /// <summary>
        /// Loops over and over sounds.
        /// </summary>
        private void LoopPlay()
        {
            if (activeStateSoundData != null && activeStateSoundData.Loop)
            {
                specificStateSoundTimers[activeStateSoundData.State] = PlayAgain();

                audioPlayers[sampleIndex % audioPlayers.Count].clip = NextClip();
                audioPlayers[sampleIndex % audioPlayers.Count].Play();
                sampleIndex++;
                sampleIndex = sampleIndex % audioPlayers.Count;

                StartCoroutine(specificStateSoundTimers[activeStateSoundData.State]);
            }
        }

		private IEnumerator LoopPlayEvent(float offset)
		{
			eventInstance.start();
			//yield return new WaitUntil(PlaybackStopped);
			yield return new WaitForSeconds(offset);
			StartCoroutine(LoopPlayEvent(offset));
		}

        /// <summary>
        /// Raises flag to play sound once again.
        /// </summary>
        /// <returns></returns>
        private IEnumerator PlayAgain()
        {
            yield return new WaitForSeconds(activeStateSoundData.LoopOffset);
            LoopPlay();
        }

		private bool PlaybackStopped()
		{
			FMOD.Studio.PLAYBACK_STATE playbackState;
			eventInstance.getPlaybackState(out playbackState);
			return playbackState == FMOD.Studio.PLAYBACK_STATE.STOPPED;
		}
    }
}
