using PlayerComponents;
using UnityEngine;

namespace Enemies
{
    public class EnemySound : EnemyComponent, IAudioSource
    {
        public AudioSource AudioSource { get; private set; }

        [SerializeField] private AudioClip deathClip;
        [SerializeField] private AudioClip stunEndClip;

        protected override void Awake()
        {
            base.Awake();
            AudioSource = GetComponentInChildren<AudioSource>();
        }

        private void Start()
        {
            Enemy.OnDeath += OnDeath;
            Enemy.OnStun += OnStun;
        }

        private void OnStun(bool state) => PlayFx(state ? deathClip : stunEndClip);
        private void OnDeath() => PlayFx(deathClip);
        public void PlayFx(AudioClip clip) => AudioSource.PlayOneShot(clip);

        private void OnDestroy()
        {
            if (Enemy == null) return;
            Enemy.OnStun -= OnStun;
            Enemy.OnDeath -= OnDeath;
        }
    }
}