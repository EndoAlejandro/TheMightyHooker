using System;
using Enemies;
using Levels;
using Pooling;
using UnityEngine;

namespace PlayerComponents
{
    [RequireComponent(typeof(EnvironmentCheck))]
    public class Player : PooledMonoBehaviour, IDie
    {
        public event Action OnJump;
        public event Action OnLanding;
        public event Action OnHooking;
        public event Action OnDeath;
        public event Action OnShooting;
        public event Action OnSlimeBlock;

        [SerializeField] private Transform hookAnchor;

        private new Rigidbody2D rigidbody;
        private new Collider2D collider;
        private EnvironmentCheck environmentCheck;

        public Transform HookAnchor => hookAnchor;
        public Level Level { get; private set; }
        public float InitialGravity { get; private set; }
        public bool IsAlive { get; private set; }
        public bool IsFacingRight { get; set; } = true;
        public bool IsGrounded { get; private set; }
        public bool IsTouchingWall { get; private set; }
        public bool IsSliding { get; set; }
        public bool IsHooking { get; private set; }

        private void Awake()
        {
            environmentCheck = GetComponent<EnvironmentCheck>();
            rigidbody = GetComponent<Rigidbody2D>();
            collider = GetComponent<Collider2D>();
            InitialGravity = rigidbody.gravityScale;
        }

        private void OnEnable()
        {
            IsAlive = true;
            collider.enabled = true;
            rigidbody.gravityScale = InitialGravity;
            rigidbody.velocity = Vector2.zero;
        }

        private void FixedUpdate()
        {
            IsGrounded = environmentCheck.Grounded && rigidbody.velocity.y < 0.1f;
            IsTouchingWall = environmentCheck.CheckWalls(IsFacingRight);
        }

        public void Jump() => OnJump?.Invoke();
        public void Land() => OnLanding?.Invoke();
        public void Shot() => OnShooting?.Invoke();
        public void SlimeBlock() => OnSlimeBlock?.Invoke();

        public void Hooking(bool isHooking)
        {
            IsHooking = isHooking;
            if (isHooking)
                OnHooking?.Invoke();
        }

        public void Die()
        {
            if (!IsAlive) return;
            IsAlive = false;
            collider.enabled = false;
            OnDeath?.Invoke();
        }

        public void DeSpawn() => ReturnToPool();

        public void AssignLevel(Level createdLevel) => Level = createdLevel;
    }
}