using System.Collections;
using UnityEngine;

namespace PlayerComponents
{
    public class Hooking : PlayerComponent
    {
        [SerializeField] private float hookRate = 0.5f;
        [SerializeField] private float hookSpeed = 13f;
        [SerializeField] private float hookResidualSpeed = 5f;
        [SerializeField] private float rotationSpeed = 10f;

        [SerializeField] private float toleranceAngle = 45f;
        [SerializeField] private float detectionRange = 7f;
        [SerializeField] private LayerMask layerMask;

        private float hookTime;
        private float initialGravity;

        private LineRenderer lineRenderer;

        private Collider2D[] collisions;

        protected override void Awake()
        {
            base.Awake();
            lineRenderer = GetComponentInChildren<LineRenderer>();
            collisions = new Collider2D[100];
        }

        private void Start()
        {
            lineRenderer.enabled = false;

            initialGravity = Rigidbody.gravityScale;
            hookTime = hookRate;
            Hook.transform.localPosition = new Vector3(detectionRange, 0f, 0f);
        }

        private void Update()
        {
            if (GameManager.IsPaused || !Player.IsAlive) return;

            hookTime -= Time.deltaTime;
            RotateCrossHair();
        }

        private void RotateCrossHair()
        {
            var offset = Player.IsFacingRight ? 0 : 180f;

            var targetAngle = Mathf.Abs(Input.Movement.magnitude) > 0
                ? Vector2.SignedAngle(Vector2.right, Input.Movement.normalized)
                : offset;

            var angle = Mathf.LerpAngle(Player.HookAnchor.localRotation.eulerAngles.z, targetAngle,
                rotationSpeed * Time.deltaTime);

            Player.HookAnchor.localRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        }

        private void FixedUpdate()
        {
            if (GameManager.IsPaused) return;
            if (!Input.Hook || !(hookTime <= 0f) || Player.IsHooking) return;

            var socketInRange = SelectHookInRange();

            if (socketInRange == null)
                MissedHook();
            else
                StartCoroutine(HookPulling(socketInRange.transform.position));
        }

        private void MissedHook() => hookTime = hookRate;

        private IEnumerator HookPulling(Vector3 target)
        {
            Player.Hooking(true);
            Rigidbody.gravityScale = 0f;
            Rigidbody.velocity = Vector2.zero;
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(1, target);

            var lastDistance = Vector3.Distance(target, transform.position);
            var currentDistance = lastDistance;
            var direction = (target - transform.position).normalized;

            while (currentDistance <= lastDistance && Input.Hook)
            {
                lineRenderer.SetPosition(0, lineRenderer.transform.position);
                Rigidbody.velocity = direction * hookSpeed;
                lastDistance = currentDistance;
                yield return new WaitForFixedUpdate();
                currentDistance = Vector3.Distance(target, transform.position);
            }

            Player.Hooking(false);
            lineRenderer.enabled = false;
            hookTime = hookRate;
            Rigidbody.gravityScale = initialGravity;
            Rigidbody.velocity = Rigidbody.velocity.normalized * hookResidualSpeed;
        }

        private HookSocket SelectHookInRange()
        {
            var results = Physics2D.OverlapCircleNonAlloc(Player.HookAnchor.position, detectionRange, collisions);

            var minAngle = float.MaxValue;
            HookSocket closestSocket = null;

            for (int i = 0; i < results; i++)
            {
                if (!collisions[i].TryGetComponent(out HookSocket socket)) continue;

                var position = transform.position;
                var hookDirection = GetHookDirection();
                var distance = (socket.transform.position - position).magnitude;
                var socketDirection = (socket.transform.position - position).normalized;

                var angle = Vector2.Angle(hookDirection, socketDirection);

                if (!(angle < toleranceAngle) || !(angle < minAngle)) continue;
                if (Physics2D.Linecast(Player.HookAnchor.position, socket.transform.position, layerMask)) continue;
                // if (Physics2D.Raycast(Player.HookAnchor.position, socketDirection, distance, layerMask)) continue;

                closestSocket = socket;
                minAngle = angle;
            }

            return closestSocket;
        }
    }
}