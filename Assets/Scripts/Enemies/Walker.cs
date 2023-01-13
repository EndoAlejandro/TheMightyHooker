using System;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(EnvironmentCheck))]
    public class Walker : Enemy
    {
        [Header("Movement")] [SerializeField] private float speed;

        private EnvironmentCheck environmentCheck;

        private bool isTouchingWall;
        private int direction;

        protected override void Awake()
        {
            base.Awake();
            environmentCheck = GetComponent<EnvironmentCheck>();
        }

        private void FixedUpdate()
        {
            if (IsStunned) return;

            Grounded = environmentCheck.Grounded;

            if (!Grounded)
            {
                Rigidbody.velocity = new Vector2(0f, Rigidbody.velocity.y);
                return;
            }

            Movement();
        }

        protected override void Movement()
        {
            if (environmentCheck.CheckWalls(IsFacingRight))
                IsFacingRight = !IsFacingRight;
            else
            {
                if (environmentCheck.LeftGrounded && !environmentCheck.RightGrounded)
                    IsFacingRight = false;
                if (!environmentCheck.LeftGrounded && environmentCheck.RightGrounded)
                    IsFacingRight = true;
            }

            direction = IsFacingRight ? 1 : -1;
            Rigidbody.velocity = new Vector2(speed * direction, Rigidbody.velocity.y);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = debugColor;
            var collider = GetComponent<BoxCollider2D>();
            Gizmos.DrawCube(collider.bounds.center, collider.bounds.size);
        }
        [ColorUsage(true, false)] public Color debugColor;
    }
}