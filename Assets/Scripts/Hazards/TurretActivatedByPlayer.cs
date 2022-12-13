using UnityEngine;

namespace Hazards
{
    public class TurretActivatedByPlayer : Turret
    {
        [Header("Detection")] [SerializeField] private Transform aimLimit;
        [SerializeField] private LayerMask detectionLayerMask;

        private bool playerInRange;
        private bool activated;

        private void FixedUpdate()
        {
            if (currentShootingTime > 0) return;
            playerInRange = Physics2D.Linecast(transform.position + direction, aimLimit.position, detectionLayerMask);
            if (playerInRange)
            {
                if (!activated) Renderer.sprite = activeSprite;
                activated = true;
                Shoot();
            }
            else
            {
                if (activated) Renderer.sprite = unActiveSprite;
                activated = false;
            }
        }

        private void OnDrawGizmos()
        {
            if (aimLimit == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, aimLimit.position);
        }
    }
}