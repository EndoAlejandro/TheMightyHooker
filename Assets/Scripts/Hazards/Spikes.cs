using Levels;
using PlayerComponents;
using UnityEngine;

namespace Hazards
{
    public abstract class Spikes : MonoBehaviour
    {
        [SerializeField] private GameObject active;
        [SerializeField] private GameObject unActive;

        [SerializeField] private BoxCollider2D activeCollider;
        [SerializeField] private BoxCollider2D unActiveCollider;

        private Level level;
        
        public bool IsActive { get; protected set; }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.TryGetComponent(out Player player)) return;
            if (IsActive) ActiveTriggerValidation(player);
            else UnActiveTriggerValidation(player);
        }

        protected virtual void SetSpikesState(bool state)
        {
            IsActive = state;
            
            active.SetActive(state);
            activeCollider.enabled = state;

            unActive.SetActive(!state);
            unActiveCollider.enabled = !state;
        }

        protected abstract void UnActiveTriggerValidation(Player player);

        private void ActiveTriggerValidation(Player player) => player.Die();
    }
}