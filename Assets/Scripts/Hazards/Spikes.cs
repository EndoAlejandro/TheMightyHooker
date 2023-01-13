using System;
using Enemies;
using PlayerComponents;
using UnityEngine;

namespace Hazards
{
    public class Spikes : MonoBehaviour
    {
        public virtual bool IsActive { get; protected set; }
        private void Awake() => IsActive = true;

        protected virtual void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.TryGetComponent(out Player player)) return;
            KillPLayer(player);
        }

        protected void KillPLayer(Player player) => player.Die();
        protected void KillEntity(IDie entity) => entity.Die();

        private void OnDrawGizmos()
        {
            Gizmos.color = IsActive ? debugColor : secondDebugColor;
            var collider = GetComponent<Collider2D>();
            Gizmos.DrawCube(collider.bounds.center, collider.bounds.size);
        }

        [ColorUsage(true, false)] public Color debugColor;
        [ColorUsage(true, false)] public Color secondDebugColor;
    }
}