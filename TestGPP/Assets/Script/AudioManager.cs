using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Stack.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        [SerializeField]
        AudioLibrarySFX librarySFX;

        [SerializeField]
        GameObject prefabAudioSource;

        Dictionary<string, AudioSource> audioSources = new Dictionary<string, AudioSource>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);

            Instance = this;
        }

        public void PlaySFX(string SFX_Name, float volume = 1f)
        {
            // Find sound
            AudioSFX audioSFX = new AudioSFX();
            bool isAudioSFX = false;
            for (int i = 0; i < librarySFX.audios.Length; i++)
            {
                if (librarySFX.audios[i].ClipName == SFX_Name)
                {
                    audioSFX = librarySFX.audios[i];
                    isAudioSFX = true;
                    break;
                }
            }

            if (!isAudioSFX)
            {
                Debug.LogError($"Audio with name {SFX_Name} not found!");
                return;
            }

            AudioSource audioSource;
            if (audioSources.ContainsKey(SFX_Name))
            {
                audioSource = audioSources[SFX_Name];
            }
            else
            {
                audioSource = Instantiate(prefabAudioSource, transform.position, Quaternion.identity, transform).GetComponent<AudioSource>();
                audioSource.name = SFX_Name;
                audioSources.Add(SFX_Name, audioSource);
            }

            if (audioSFX.AudioClip)
            {
                audioSource.clip = audioSFX.AudioClip;
            }
            else
            {
                Debug.LogError($"There is no clip attached to {SFX_Name}!");
                return;
            }

            // Set properties
            audioSource.volume = volume * audioSFX.IntensityFactor;

            if (audioSFX.IncrementPitch)
            {
                if (!audioSFX.BreakIncrementPitch)
                {
                    if (audioSFX.IncrementPitchMax > audioSource.pitch)
                    {
                        audioSource.pitch += audioSFX.IncrementPitchValue;
                    }
                }
                else
                {
                    audioSource.pitch = Random.Range(audioSFX.PitchMin, audioSFX.PitchMax);
                    audioSFX.BreakIncrementPitch = false;
                }
            }
            else
            {
                audioSource.pitch = Random.Range(audioSFX.PitchMin, audioSFX.PitchMax);
            }

            audioSource.Play();

        }

        public void BreakIncrementPitch(string SFX_Name)
        {
            for (int i = 0; i < librarySFX.audios.Length; i++)
            {
                if (librarySFX.audios[i].ClipName == SFX_Name)
                {
                    librarySFX.audios[i].BreakIncrementPitch = true;
                    break;
                }
            }
        }
    }
}
