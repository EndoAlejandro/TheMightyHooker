using System.Collections;
using PlayerComponents;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hazards
{
    public class SpikesActivatedByPlayer : Spikes
    {
        [Header("Spike type")]
        [SerializeField] private bool turnOffAfterTime = true;

        [Header("Timers")]
        [SerializeField] private float activationDelay = 1f;
        [SerializeField] private float deActivationDelay = 2f;

        [Header("Visual")]
        [SerializeField] private Sprite unActiveSprite;
        [SerializeField] private float shakeMagnitude = 0.2f;

        private bool isActivating;
        private bool isActive;

        private Vector3 initialPosition;

        private new SpriteRenderer renderer;
        private Sprite activeSprite;

        private void Awake()
        {
            renderer = GetComponentInChildren<SpriteRenderer>();
            activeSprite = renderer.sprite;
        }

        private void Start()
        {
            initialPosition = transform.localPosition;
            SetSpikesState(false);
        }

        private void SetSpikesState(bool state)
        {
            isActive = state;
            renderer.sprite = isActive ? activeSprite : unActiveSprite;
        }

        protected override void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.TryGetComponent(out Player player)) return;
            if (isActive)
                KillPLayer(player);
            else if (!isActivating)
                StartCoroutine(ActivationCycle());
        }

        private IEnumerator ActivationCycle()
        {
            yield return ActivateSpikes();
            if (turnOffAfterTime)
                yield return ChangeStateAfterDelay(false);
        }

        private IEnumerator ActivateSpikes()
        {
            var currentTime = 0f;
            while (currentTime < activationDelay)
            {
                var x = Random.Range(-1f, 1f) * shakeMagnitude + initialPosition.x;
                var y = Random.Range(-1f, 1f) * shakeMagnitude + initialPosition.y;

                transform.localPosition = new Vector3(x, y, transform.localPosition.z);

                currentTime += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = initialPosition;
            SetSpikesState(true);
            isActivating = false;
            yield return null;
        }

        private IEnumerator ChangeStateAfterDelay(bool status)
        {
            var waitTime = status ? activationDelay : deActivationDelay;
            yield return new WaitForSeconds(waitTime);
            SetSpikesState(status);
        }
    }
}