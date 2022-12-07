using UnityEngine;

namespace Hazards
{
    public abstract class Turret : MonoBehaviour
    {
        [Header("Children")] [SerializeField] protected Transform barrel;
        [SerializeField] protected Projectile projectilePrefab;

        [Header("Shooting")] [SerializeField] private float shootingRate = 1.5f;
        [SerializeField] private float projectileSpeed = 7f;

        private new SpriteRenderer renderer;

        protected Vector3 direction;
        protected float currentShootingTime;

        protected virtual void Awake() => renderer = GetComponent<SpriteRenderer>();

        protected virtual void Start()
        {
            ResetShootingTimer();
            SetDirection();
        }

        protected virtual void SetDirection()
        {
            var x = renderer.flipX ? -1 : 1;
            direction = new Vector2(x, 0f);
        }

        protected virtual void Update() => currentShootingTime -= Time.deltaTime;
        
        private void ResetShootingTimer() => currentShootingTime = shootingRate;

        protected void Shoot()
        {
            Debug.Log("Shoot");
            var projectile = projectilePrefab.Get<Projectile>(barrel.position, Quaternion.identity);
            projectile.Initialize(direction, projectileSpeed);
            ResetShootingTimer();
        }
    }
}