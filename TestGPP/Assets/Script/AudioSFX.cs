using System;
using UnityEngine;

namespace Game.Stack.Audio
{
    [Serializable]
    public class AudioSFX
    {
        [SerializeField]
        private string clipName;
        public string ClipName { get => clipName; set => clipName = value; }

        [SerializeField]
        private AudioClip audioClip;
        public AudioClip AudioClip { get => audioClip; set => audioClip = value; }

        [SerializeField]
        [Range(0f, 1f)]
        private float intensityFactor;
        public float IntensityFactor { get => intensityFactor; set => intensityFactor = value; }

        [SerializeField]
        private float pitchMin;
        public float PitchMin { get => pitchMin; set => pitchMin = value; }

        [SerializeField]
        private float pitchMax;
        public float PitchMax { get => pitchMax; set => pitchMax = value; }

        [SerializeField]
        private bool incrementPitch;
        public bool IncrementPitch { get => incrementPitch; set => incrementPitch = value; }

        [SerializeField]
        private float incrementPitchValue;
        public float IncrementPitchValue { get => incrementPitchValue; set => incrementPitchValue = value; }

        [SerializeField]
        private float incrementPitchMax;
        public float IncrementPitchMax { get => incrementPitchMax; set => incrementPitchMax = value; }

        private bool breakIncrementPitch = false;
        public bool BreakIncrementPitch { get => breakIncrementPitch; set => breakIncrementPitch = value; }

    }
}
