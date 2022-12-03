using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class Flying : Enemy
    {
        private enum MovementType
        {
            Static,
            Horizontal,
            Vertical,
            Universal,
        }

        private Vector2 direction;

        [SerializeField] private MovementType movementType;
        [SerializeField] private float speed;
        [SerializeField] private Transform body;
        [SerializeField] private LayerMask collisionLayerMask;

        protected override void Start()
        {
            base.Start();
            SetDirection();
        }

        private void SetDirection()
        {
            var x = 0f;
            var y = 0f;

            switch (movementType)
            {
                case MovementType.Horizontal:
                    x = Random.Range(0f, 1f) > 0.5f ? 1 : -1;
                    break;
                case MovementType.Vertical:
                    y = Random.Range(0f, 1f) > 0.5f ? 1 : -1;
                    break;
                case MovementType.Universal:
                    y = Random.Range(0f, 1f) > 0.5f ? 1 : -1;
                    x = Random.Range(0f, 1f) > 0.5f ? 1 : -1;
                    break;
            }

            direction = new Vector2(x, y).normalized;
        }

        private void Update() => IsFacingRight = Rigidbody.velocity.x > 0;
        private void FixedUpdate() => Movement();
        private void Movement() => Rigidbody.velocity = direction * speed;

        protected override void OnCollisionEnter2D(Collision2D col)
        {
            base.OnCollisionEnter2D(col);
            if ((collisionLayerMask & 1 << col.gameObject.layer) != 1 << col.gameObject.layer) return;

            if (Math.Abs(col.contacts[0].point.x - body.position.x) > 0.35f) direction.x *= -1;
            else if (Math.Abs(col.contacts[0].point.y - body.position.y) > 0.10f) direction.y *= -1;
        }
    }
}