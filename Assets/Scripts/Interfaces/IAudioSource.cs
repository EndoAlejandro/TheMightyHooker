using UnityEngine;

namespace PlayerComponents
{
    public interface IAudioSource
    {
        public AudioSource AudioSource { get; }
        public void PlayFx(AudioClip clip);
    }
}