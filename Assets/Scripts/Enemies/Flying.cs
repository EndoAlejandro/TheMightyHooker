﻿using CustomUtils;
using Hazards;
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

        [Header("Movement")] [SerializeField] private MovementType movementType;
        [SerializeField] private float speed;

        [Header("Detection system")] [SerializeField]
        private float collisionDetectionRange = 0.05f;

        [SerializeField] private LayerMask collisionLayerMask;

        [SerializeField] private Transform topPoint;
        [SerializeField] private Transform bottomPoint;
        [SerializeField] private Transform leftPoint;
        [SerializeField] private Transform rightPoint;

        private Collider2D[] collisions;

        protected override void Awake()
        {
            base.Awake();
            collisions = new Collider2D[10];
        }

        protected override void Start()
        {
            base.Start();
            SetDirection();
        }

        private void SetDirection()
        {
            var x = 0f;
            var y = 0f;
            var constraints = RigidbodyConstraints2D.FreezeRotation;
            switch (movementType)
            {
                case MovementType.Horizontal:
                    constraints |= RigidbodyConstraints2D.FreezePositionY;
                    x = Random.Range(0f, 1f) > 0.5f ? 1 : -1;
                    break;
                case MovementType.Vertical:
                    constraints |= RigidbodyConstraints2D.FreezePositionX;
                    y = Random.Range(0f, 1f) > 0.5f ? 1 : -1;
                    break;
                case MovementType.Universal:
                    y = Random.Range(0f, 1f) > 0.5f ? 1 : -1;
                    x = Random.Range(0f, 1f) > 0.5f ? 1 : -1;
                    break;
                case MovementType.Static:
                    constraints = RigidbodyConstraints2D.FreezeAll;
                    break;
            }

            Rigidbody.constraints = constraints;
            direction = new Vector2(x, y).normalized;
        }

        private void FixedUpdate()
        {
            if (IsStunned) return;
            IsFacingRight = Rigidbody.velocity.x > 0;
            Movement();
        }

        private void Update()
        {
            if (IsStunned) return;
            CheckCardinalPoints();
        }

        private void CheckCardinalPoints()
        {
            if (CheckWalls(topPoint))
                direction = direction.With(y: Mathf.Abs(direction.y) * -1);
            else if (CheckWalls(bottomPoint))
                direction = direction.With(y: Mathf.Abs(direction.y));
            if (CheckWalls(rightPoint))
                direction = direction.With(x: Mathf.Abs(direction.x) * -1);
            else if (CheckWalls(leftPoint))
                direction = direction.With(x: Mathf.Abs(direction.x));
        }

        protected override void Movement() => Rigidbody.velocity = direction * speed;

        private bool CheckWalls(Transform checkPoint)
        {
            var results =
                Physics2D.OverlapCircleNonAlloc(checkPoint.position, collisionDetectionRange, collisions,
                    collisionLayerMask);

            var lastResults = results;
            for (int i = 0; i < results; i++)
            {
                if (collisions[i].TryGetComponent(out Spikes spikes))
                    lastResults += spikes.IsActive ? 0 : -1;
            }

            return lastResults > 0;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = cardinalColor;
            Gizmos.DrawSphere(topPoint.position, collisionDetectionRange);
            Gizmos.DrawSphere(bottomPoint.position, collisionDetectionRange);
            Gizmos.DrawSphere(leftPoint.position, collisionDetectionRange);
            Gizmos.DrawSphere(rightPoint.position, collisionDetectionRange);
            Gizmos.color = debugColor;
            var collider = GetComponent<Collider2D>();
            Gizmos.DrawSphere(collider.bounds.center, collider.bounds.size.x / 2);
        }

        [ColorUsage(true, false)] public Color debugColor;
        [ColorUsage(true, false)] public Color cardinalColor;
    }
}