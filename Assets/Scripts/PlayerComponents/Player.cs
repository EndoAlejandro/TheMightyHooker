using System;
using Enemies;
using UnityEngine;

namespace PlayerComponents
{
    [RequireComponent(typeof(EnvironmentCheck))]
    public class Player : MonoBehaviour, IDie
    {
        public event Action OnJump;
        public event Action OnLanding;
        public event Action OnHooking;
        public event Action OnDeath;
        public event Action OnShooting;

        [SerializeField] private Transform hookAnchor;

        private EnvironmentCheck environmentCheck;
        public Transform HookAnchor => hookAnchor;

        public bool IsAlive { get; private set; }
        public bool IsFacingRight { get; set; } = true;
        public bool IsGrounded { get; private set; }
        public bool IsTouchingWall { get; private set; }
        public bool IsSliding { get; set; }
        public bool IsHooking { get; set; }

        private Rigidbody2D rigidbody;

        private void Awake()
        {
            environmentCheck = GetComponent<EnvironmentCheck>();
            rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Start() => IsAlive = true;

        private void FixedUpdate()
        {
            IsGrounded = environmentCheck.Grounded && rigidbody.velocity.y < 0.1f;
            IsTouchingWall = environmentCheck.CheckWalls(IsFacingRight);
        }

        public void Jump() => OnJump?.Invoke();
        public void Land() => OnLanding?.Invoke();
        public void Shot() => OnShooting?.Invoke();

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
            OnDeath?.Invoke();
        }
    }
}