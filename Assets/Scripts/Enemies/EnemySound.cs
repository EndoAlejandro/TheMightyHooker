using PlayerComponents;
using UnityEngine;

namespace Enemies
{
    public class EnemySound : EnemyComponent, IAudioSource
    {
        public AudioSource AudioSource { get; private set; }

        [SerializeField] private AudioClip deathClip;

        protected override void Awake()
        {
            base.Awake();
            AudioSource = GetComponentInChildren<AudioSource>();
        }

        private void Start() => Enemy.OnDeath += OnDeath;
        private void OnDeath() => PlayFx(deathClip);
        public void PlayFx(AudioClip clip) => AudioSource.PlayOneShot(clip);
        private void OnDestroy() => Enemy.OnDeath -= OnDeath;
    }
}