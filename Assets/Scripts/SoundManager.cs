using System;
using CustomUtils;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    [Header("Audio Mixer Group")] [SerializeField]
    private AudioMixerGroup masterGroup;

    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup fXGroup;

    [Header("Audio Source")] [SerializeField]
    private AudioSource musicAudioSource;

    [SerializeField] private AudioSource fxAudioSource;
    [SerializeField] private AudioSource openDoorAudioSource;

    [Header("Audio Clips")] [SerializeField]
    private AudioClip mainMenuClip;

    [SerializeField] private AudioClip pickUpClip;
    [SerializeField] private AudioClip openDoorClip;
    [SerializeField] private AudioClip uiNavigate;
    [SerializeField] private AudioClip uiSubmit;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);

        if (GameManager.Instance == null)
            GameManager.CreateGameManager();
    }

    private void Start()
    {
        SetMixerGroupVolume(masterGroup, SaveSystem.GetVolume(SaveSystem.PrefsField.Master));
        SetMixerGroupVolume(musicGroup, SaveSystem.GetVolume(SaveSystem.PrefsField.Music));
        SetMixerGroupVolume(fXGroup, SaveSystem.GetVolume(SaveSystem.PrefsField.Fx));
    }

    public void PlayPickUp() => PlayFx(pickUpClip);
    public void PlayOpenDoor() => openDoorAudioSource.PlayOneShot(openDoorClip);
    public void PlayNavigate() => PlayFx(uiNavigate);
    public void PlaySubmit() => PlayFx(uiSubmit);
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

    private void PlayFx(AudioClip clip)
    {
        fxAudioSource.pitch = 1f;
        fxAudioSource.PlayOneShot(clip);
    }

    public static void SetMixerGroupVolume(AudioMixerGroup group, float value) =>
        group.audioMixer.SetFloat(group.name + "Volume", SoundManager.FromNormalizedToLog(value));

    private static float FromNormalizedToLog(float value) => Mathf.Log10(value / 10) * 20;
}