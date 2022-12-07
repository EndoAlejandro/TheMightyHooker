using System.Collections;
using CustomUtils;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [Header("Audio Source")] [SerializeField]
    private AudioSource musicAudioSource;

    [SerializeField] private AudioSource fxAudioSource;

    [Header("Audio Clips")] [SerializeField]
    private AudioClip mainMenuClip;

    [SerializeField] private AudioClip pickUpClip;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void PlayPickUp() => PlayFx(pickUpClip);
    public void PlayMainMenu() => PlayMusic(mainMenuClip);

    public void PlayMusic(AudioClip clip)
    {
        if ((musicAudioSource.clip != null ? musicAudioSource.clip.name : null) == clip.name &&
            musicAudioSource.isPlaying) return;

        if (musicAudioSource.isPlaying) musicAudioSource.Stop();

        musicAudioSource.clip = clip;
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }

    public void PlayFx(AudioClip clip)
    {
        fxAudioSource.pitch = 1f;
        fxAudioSource.PlayOneShot(clip);
    }

    public void PlayFx(AudioClip clip, float pitch)
    {
        fxAudioSource.Stop();
        fxAudioSource.pitch = pitch;
        fxAudioSource.PlayOneShot(clip);
        var duration = (1 - pitch) * 2 * clip.length;
        StartCoroutine(PlayWithPitch(duration));
    }

    private IEnumerator PlayWithPitch(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        fxAudioSource.pitch = 1f;
    }
}