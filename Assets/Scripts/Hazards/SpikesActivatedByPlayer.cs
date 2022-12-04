using System.Collections;
using PlayerComponents;
using UnityEngine;

namespace Hazards
{
    public class SpikesActivatedByPlayer : Spikes
    {
        [SerializeField] private float activationDelay = 1f;
        [SerializeField] private float deActivationDelay = 2f;

        [SerializeField] private float shakeMagnitude = 0.2f;

        [SerializeField] private bool turnOffAfterTime = true;

        private bool activating;

        private Vector3 initialPosition;

        private void Start()
        {
            initialPosition = transform.localPosition;
            SetSpikesState(false);
        }

        protected override void UnActiveTriggerValidation(Player player)
        {
            if (activating) return;
            StartCoroutine(ActivationCycle());
        }

        private IEnumerator ActivationCycle()
        {
            yield return ActivateSpikes();
            if (turnOffAfterTime)
                yield return ChangeSpikesStateAfterDelay(false);
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
            activating = false;
            yield return null;
        }

        private IEnumerator ChangeSpikesStateAfterDelay(bool status)
        {
            var waitTime = status ? activationDelay : deActivationDelay;
            yield return new WaitForSeconds(waitTime);
            SetSpikesState(status);
        }
    }
}