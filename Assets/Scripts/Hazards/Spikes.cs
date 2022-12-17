using PlayerComponents;
using UnityEngine;

namespace Hazards
{
    public class Spikes : MonoBehaviour
    {
        public virtual bool IsActive { get; protected set; }
        
        protected virtual void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.TryGetComponent(out Player player)) return;
            KillPLayer(player);
        }

        protected void KillPLayer(Player player) => player.Die();
    }
}