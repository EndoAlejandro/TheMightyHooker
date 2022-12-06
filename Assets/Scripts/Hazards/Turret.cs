using PlayerComponents;
using UnityEngine;

namespace Hazards
{
    public class Turret : MonoBehaviour
    {
        [SerializeField] private Transform aimLimit;
        [SerializeField] private Transform barrel;

        [SerializeField] private Projectile projectilePrefab;

        [SerializeField] private float shootingRate = 1.5f;
        [SerializeField] private float projectileSpeed = 7f;

        [SerializeField] private LayerMask detectionLayerMask;

        private Vector3 origin = Vector3.zero;
        private Vector3 direction;

        private new SpriteRenderer renderer;
        private new Collider2D collider;

        private float currentShootingTime;

        private void Awake()
        {
            renderer = GetComponent<SpriteRenderer>();
            collider = GetComponent<Collider2D>();
        }

        private void Start()
        {
            ResetShootingTimer();
            CalculateOrigin();
        }

        private void Update() => currentShootingTime -= Time.deltaTime;

        private void FixedUpdate()
        {
            if (currentShootingTime > 0) return;
            var result = Physics2D.Linecast(transform.position + direction, aimLimit.position, detectionLayerMask);
            if (result)
                Shoot();
        }

        private void Shoot()
        {
            var projectile = projectilePrefab.Get<Projectile>(barrel.position, Quaternion.identity);
            projectile.Initialize(direction, projectileSpeed);
            ResetShootingTimer();
            Debug.Log("Shoot!");
        }

        private void ResetShootingTimer() => currentShootingTime = shootingRate;

        private void CalculateOrigin()
        {
            var x = renderer.flipX ? collider.bounds.min.x : collider.bounds.max.x;
            direction = new Vector2(-x, 0f).normalized;
            origin = new Vector2(x, collider.bounds.center.y);
        }

        private void OnDrawGizmos()
        {
            if (aimLimit == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, aimLimit.position);
        }
    }
}