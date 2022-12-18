using Interfaces;
using PlayerComponents;
using Pooling;
using UnityEngine;

namespace Levels
{
    public class PickUp : MonoBehaviour, IResettable
    {
        [SerializeField] private PoolAfterSeconds pickUpFxPrefab;

        private Level level;

        public void Setup(Level level) => this.level = level;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.transform.TryGetComponent(out Player player)) return;

            level.PickUpGem();
            gameObject.SetActive(false);
            pickUpFxPrefab.Get<PoolAfterSeconds>(transform.position, Quaternion.identity);
        }

        public void Reset()
        {
            if (GameManager.AssistMode) return;
            gameObject.SetActive(true);
        }
    }
}