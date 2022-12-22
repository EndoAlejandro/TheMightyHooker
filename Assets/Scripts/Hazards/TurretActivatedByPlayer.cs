using UnityEngine;

namespace Hazards
{
    public class TurretActivatedByPlayer : Turret
    {
        [Header("Detection")] [SerializeField] private Transform aimLimit;
        [SerializeField] private LayerMask detectionLayerMask;

        private LineRenderer lineRenderer;
        private RaycastHit2D hit;

        private bool playerInRange;
        private bool activated;

        protected override void Awake()
        {
            base.Awake();
            lineRenderer = GetComponentInChildren<LineRenderer>();
        }

        protected override void Start()
        {
            base.Start();

            lineRenderer.SetPosition(0, transform.position);
            // lineRenderer.SetPosition(1, aimLimit.position);
        }

        private void FixedUpdate()
        {
            hit = Physics2D.Linecast(transform.position + Direction, aimLimit.position, detectionLayerMask);
            //playerInRange = Physics2D.Raycast(transform.position, Direction, Vector3.Distance(transform.position, aimLimit.position), detectionLayerMask);
            if (hit)
            {
                lineRenderer.SetPosition(1, hit.point);
                if (CurrentShootingTime > 0) return;

                if (!activated) Renderer.sprite = activeSprite;
                activated = true;
                Shoot();
            }
            else
            {
                lineRenderer.SetPosition(1, aimLimit.position);
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