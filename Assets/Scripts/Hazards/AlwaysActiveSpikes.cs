using System;
using PlayerComponents;

namespace Hazards
{
    public class AlwaysActiveSpikes : Spikes
    {
        private void Start() => IsActive = true;

        protected override void UnActiveTriggerValidation(Player player)
        {
        }

    }
}