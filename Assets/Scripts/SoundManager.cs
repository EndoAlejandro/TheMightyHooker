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
    [SerializeField] private AudioClip uiNavigate;
    [SerializeField] private AudioClip uiSubmit;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        if (GameManager.Instance == null)
            GameManager.CreateGameManager();
    }

    public void PlayPickUp() => PlayFx(pickUpClip);
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
}