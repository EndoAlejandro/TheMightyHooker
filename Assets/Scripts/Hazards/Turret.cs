using UnityEngine;

namespace Hazards
{
    public abstract class Turret : MonoBehaviour
    {
        [Header("Display")] [SerializeField] protected Sprite unActiveSprite;
        [SerializeField] protected Sprite activeSprite;

        [Header("Children")] [SerializeField] protected Transform barrel;
        [SerializeField] protected Projectile projectilePrefab;

        [Header("Shooting")] [SerializeField] private float shootingRate = 1.5f;
        [SerializeField] private float projectileSpeed = 7f;

        protected SpriteRenderer Renderer;

        protected Vector3 Direction;
        protected float CurrentShootingTime;

        protected virtual void Awake() => Renderer = GetComponent<SpriteRenderer>();

        protected virtual void Start()
        {
            ResetShootingTimer();
            SetDirection();
        }

        private void SetDirection() => Direction = (barrel.position - transform.position).normalized;
        protected virtual void Update() => CurrentShootingTime -= Time.deltaTime;
        private void ResetShootingTimer() => CurrentShootingTime = shootingRate;
        protected void Shoot()
        {
            var projectile = projectilePrefab.Get<Projectile>(barrel.position, Quaternion.identity);
            projectile.Initialize(Direction, projectileSpeed);
            ResetShootingTimer();
        }
    }
}