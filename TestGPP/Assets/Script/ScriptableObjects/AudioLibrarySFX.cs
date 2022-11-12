using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Stack.Audio
{
    [CreateAssetMenu(fileName = "AudioLibrarySFX", menuName = "ScriptableObjects/Audio", order = 1)]
    public class AudioLibrarySFX : ScriptableObject
    {
        public AudioSFX[] audios;
    }
}
