using UnityEngine;

namespace Hazards
{
    public class TurretActivatedByPlayer : Turret
    {
        [Header("Detection")]
        [SerializeField] private Transform aimLimit;
        [SerializeField] private LayerMask detectionLayerMask;

        private void FixedUpdate()
        {
            if (currentShootingTime > 0) return;
            var result = Physics2D.Linecast(transform.position + direction, aimLimit.position, detectionLayerMask);
            if (result)
                Shoot();
        }

        private void OnDrawGizmos()
        {
            if (aimLimit == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, aimLimit.position);
        }
    }
}