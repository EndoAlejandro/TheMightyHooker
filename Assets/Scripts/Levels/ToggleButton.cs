using System;
using System.Collections;
using Interfaces;
using PlayerComponents;
using UnityEngine;

namespace Levels
{
    [RequireComponent(typeof(AudioSource))]
    public class ToggleButton : MonoBehaviour, IToggle
    {
        public event Action<bool> OnToggle;

        [Header("Behaviour")] [SerializeField] private bool initialState;
        [SerializeField] private bool turnOffAfterTime;
        [SerializeField] private float deActivationDelay = 2f;

        [Header("Display")] [SerializeField] private Sprite activeSprite;
        [SerializeField] private Sprite unActiveSprite;
        [SerializeField] private AudioClip clip;

        private new SpriteRenderer renderer;
        private AudioSource audioSource;

        private bool isPressed;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            renderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            audioSource.clip = clip;
            audioSource.loop = false;

            SetState(initialState, false);
        }

        private void SetState(bool state, bool playAudio = true)
        {
            isPressed = state;
            renderer.sprite = isPressed ? activeSprite : unActiveSprite;
            OnToggle?.Invoke(isPressed);
            if (playAudio) audioSource.Play();

            if (isPressed && turnOffAfterTime) StartCoroutine(TurnOffAfterDelay());
        }

        private IEnumerator TurnOffAfterDelay()
        {
            yield return new WaitForSeconds(deActivationDelay);
            SetState(false);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!isPressed && col.TryGetComponent(out Player player))
                SetState(true);
        }
    }
}