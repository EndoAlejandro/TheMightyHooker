using System;
using CustomUtils;
using Pooling;
using UnityEngine;

namespace Enemies
{
    public class EnemyAnimation : EnemyComponent
    {
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Grounded = Animator.StringToHash("Grounded");

        [SerializeField] private Transform body;
        [SerializeField] private float deathAnimationSpeed = 1f;
        [SerializeField] private float bulletTime = 0.1f;
        [SerializeField] private PoolAfterSeconds deathDust;
        [SerializeField] private Vector2 deathShake = new Vector2(0.1f, 0.025f);

        private new SpriteRenderer renderer;
        private new Collider2D collider;
        private Animator animator;
        private static readonly int Stun = Animator.StringToHash("Stun");

        protected override void Awake()
        {
            base.Awake();
            animator = body.GetComponent<Animator>();
            renderer = body.GetComponent<SpriteRenderer>();
            collider = GetComponent<Collider2D>();
        }

        private void Start()
        {
            Enemy.OnSpawn += OnSpawn;
        }

        private void OnSpawn()
        {
            collider.enabled = true;
            renderer.enabled = true;
        }

        private void OnEnable()
        {
            Enemy.OnDeath += OnDeath;
            Enemy.OnStun += OnStun;
        }

        private void Update()
        {
            if (!Enemy.IsFacingRight != renderer.flipX) renderer.flipX = !Enemy.IsFacingRight;

            var speed = Mathf.Abs(Enemy.Rigidbody.velocity.x);
            animator.SetFloat(Speed, speed);
            animator.SetBool(Grounded, Enemy.Grounded);
        }

        private void OnDeath()
        {
            collider.enabled = false;
            renderer.enabled = false;
            CameraController.Instance.CamShake(deathShake);
            deathDust.Get<PoolAfterSeconds>(transform.position, Quaternion.identity);
            StartCoroutine(Utils.DieSequence(deathAnimationSpeed, bulletTime, () => gameObject.SetActive(false)));
        }

        private void OnStun(bool state) => animator.SetBool(Stun, state);

        private void OnDisable()
        {
            if (Enemy == null) return;
            Enemy.OnDeath -= OnDeath;
            Enemy.OnStun -= OnStun;
        }

        private void OnDestroy()
        {
            if (Enemy == null) return;
            Enemy.OnSpawn -= OnSpawn;
        }
    }
}