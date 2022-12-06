using UnityEngine;

namespace Hazards
{
    public class Turret : MonoBehaviour
    {
        [SerializeField] private Projectile projectile;
        [SerializeField] private float shootingRate = 1.5f;

        [SerializeField] private LayerMask initialLayerMask;
        [SerializeField] private LayerMask detectionLayerMask;

        private Vector3 endPoint = Vector3.zero;
        private Vector3 origin = Vector3.zero;
        private Vector2 direction;

        private new SpriteRenderer renderer;
        private new Collider2D collider;

        private float currentShootingTime;

        private void Awake()
        {
            renderer = GetComponent<SpriteRenderer>();
            collider = GetComponent<Collider2D>();
        }

        private void Start() => currentShootingTime = shootingRate;

        private void Update()
        {
            if (endPoint == Vector3.zero) CalculateEndPoint();

            currentShootingTime -= Time.deltaTime;
        }

        private void FixedUpdate()
        {
            if (endPoint == Vector3.zero) return;

            if (Physics2D.Linecast(origin, endPoint, detectionLayerMask) && currentShootingTime < 0)
                Shoot();
        }

        private void Shoot() => Debug.Log("Shooting!!");

        private void CalculateEndPoint()
        {
            direction = renderer.flipX ? Vector2.left : Vector2.right;
            var x = renderer.flipX ? collider.bounds.min.x : collider.bounds.max.x;
            origin = new Vector2(x, collider.bounds.center.y);
            var hit = Physics2D.Raycast(origin, direction, float.MaxValue, initialLayerMask);
            if (hit) endPoint = hit.point;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(origin, endPoint);
        }
    }
}