using System;
using Interfaces;
using UnityEngine;

namespace Levels
{
    [RequireComponent(typeof(AudioSource))]
    public class ToggleTimer : MonoBehaviour, IToggle
    {
        public event Action<bool> OnToggle;

        [SerializeField] private float toggleTime = 5f;
        [SerializeField] private bool initialState;

        [SerializeField] private AudioClip clip;

        private AudioSource audioSource;
        
        private float currentToggleTime;
        private bool state;

        private void Awake() => audioSource = GetComponent<AudioSource>();

        private void Start()
        {
            audioSource.clip = clip;
            audioSource.loop = false;
            
            ResetTimer();
            state = !initialState;
            Toggle();
        }

        private void Update()
        {
            currentToggleTime -= Time.deltaTime;

            if (currentToggleTime < 0) Toggle();
        }

        private void ResetTimer() => currentToggleTime = toggleTime;

        private void Toggle()
        {
            ResetTimer();
            state = !state;
            audioSource.Play();
            OnToggle?.Invoke(state);
        }
    }
}