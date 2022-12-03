using System.Collections;
using PlayerComponents;
using UnityEngine;

namespace Hazards
{
    public class SpikesActivatedByPlayer : Spikes
    {
        [SerializeField] private float activationDelay = 1f;
        [SerializeField] private float deActivationDelay = 2f;

        protected override void UnActiveTriggerValidation(Player player) =>
            StartCoroutine(ActivationCycle());

        private IEnumerator ActivationCycle()
        {
            yield return ChangeSpikesStateAfterDelay(true);
            yield return ChangeSpikesStateAfterDelay(false);
        }

        private IEnumerator ChangeSpikesStateAfterDelay(bool status)
        {
            var waitTime = status ? activationDelay : deActivationDelay;
            yield return new WaitForSeconds(waitTime);
            SetSpikesState(status);
        }
    }
}