using UnityEngine;

namespace PlayerComponents
{
    public class PlayerSound : PlayerComponent, IAudioSource
    {
        public AudioSource AudioSource { get; private set; }

        [SerializeField] private AudioClip jumpClip;
        [SerializeField] private AudioClip landClip;
        [SerializeField] private AudioClip hookClip;
        [SerializeField] private AudioClip deathClip;
        [SerializeField] private AudioClip shootClip;

        protected override void Awake()
        {
            base.Awake();
            AudioSource = GetComponentInChildren<AudioSource>();
        }

        private void Start()
        {
            Player.OnJump += OnJump;
            Player.OnLanding += OnLanding;
            Player.OnHooking += OnHooking;
            Player.OnDeath += OnDeath;
            Player.OnShooting += OnShooting;
        }

        private void OnShooting() => PlayFx(shootClip);
        private void OnDeath() => PlayFx(deathClip, 0.25f);
        private void OnHooking() => PlayFx(hookClip);
        private void OnLanding() => PlayFx(landClip);
        private void OnJump() => PlayFx(jumpClip);

        public void PlayFx(AudioClip clip)
        {
            AudioSource.pitch = 1f;
            AudioSource.PlayOneShot(clip);
        }

        private void PlayFx(AudioClip clip, float pitch)
        {
            AudioSource.Stop();
            AudioSource.pitch = pitch;
            AudioSource.PlayOneShot(clip);
            // var duration = (1 - pitch) * 2 * clip.length;
        }

        private void OnDestroy()
        {
            if (Player == null) return;
            Player.OnJump -= OnJump;
            Player.OnLanding -= OnLanding;
            Player.OnHooking -= OnHooking;
            Player.OnDeath -= OnDeath;
        }
    }
}