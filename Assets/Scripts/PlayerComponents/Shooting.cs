using UnityEngine;

namespace PlayerComponents
{
    public class Shooting : PlayerComponent
    {
        [SerializeField] private Projectile projectilePrefab;

        [SerializeField] private float fireRate = 1f;
        [SerializeField] private float projectileSpeed = 7f;
        [SerializeField] private float projectileLifeTime = 1f;
        [Range(0f, 1f)] [SerializeField] private float barrelOffset = 0.5f;

        private float currentFireRate;

        private bool CanShoot => currentFireRate <= 0;

        private void Start() => ResetFireRate();

        private void Update()
        {
            if (GameManager.IsPaused || !Player.IsAlive) return;

            currentFireRate -= Time.deltaTime;

            if (CanShoot && Input.Shoot && !Player.IsHooking)
                Shoot();
        }

        private void Shoot()
        {
            var revertDirection = Player.IsSliding ? !Player.IsFacingRight : Player.IsFacingRight;
            var offsetDirection = revertDirection ? Vector3.right : Vector3.left;
            var offset = offsetDirection * barrelOffset;

            var projectile =
                projectilePrefab.Get<Projectile>(Player.HookAnchor.transform.position + offset, Quaternion.identity);
            projectile.Initialize(offsetDirection, projectileSpeed, projectileLifeTime);
            Player.Shot();
            ResetFireRate();
        }

        private void ResetFireRate() => currentFireRate = fireRate;
    }
}