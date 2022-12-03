using PlayerComponents;
using UnityEngine;

namespace Levels
{
    public class PickUp : MonoBehaviour
    {
        private Level level;

        public void Setup(Level level) => this.level = level;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.transform.TryGetComponent(out Player player)) return;

            level.PickUpGem();
            gameObject.SetActive(false);
        }
    }
}