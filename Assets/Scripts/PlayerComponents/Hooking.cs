using System.Collections;
using UnityEngine;

namespace PlayerComponents
{
    public class Hooking : PlayerComponent
    {
        [SerializeField] private Transform selectedHookDisplay;
        [SerializeField] private Transform hookPoint;

        [SerializeField] private float hookRate = 0.5f;
        [SerializeField] private float hookSpeed = 13f;
        [SerializeField] private float hookResidualSpeed = 5f;
        [SerializeField] private float rotationSpeed = 10f;

        [SerializeField] private float toleranceAngle = 45f;

        [SerializeField] private float detectionRange = 7f;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private LayerMask aimHookLayerMask;

        private Vector3 target = Vector3.zero;

        private float hookTime;

        private LineRenderer lineRenderer;
        private HookingDisplay hookingDisplay;

        private Collider2D[] collisions;
        private RaycastHit2D raycastHit2D;

        protected override void Awake()
        {
            base.Awake();
            hookingDisplay = GetComponentInChildren<HookingDisplay>();
            lineRenderer = GetComponentInChildren<LineRenderer>();
            collisions = new Collider2D[100];
        }

        private void Start()
        {
            lineRenderer.enabled = false;

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
            var offsetCheck = Player.IsSliding ? !Player.IsFacingRight : Player.IsFacingRight;
            var offset = offsetCheck ? 0 : 180f;

            var targetAngle = Mathf.Abs(InputReader.Movement.magnitude) > 0
                ? Vector2.SignedAngle(Vector2.right, InputReader.Movement.normalized)
                : offset;

            var angle = Mathf.LerpAngle(Player.HookAnchor.localRotation.eulerAngles.z, targetAngle,
                rotationSpeed * Time.deltaTime);

            Player.HookAnchor.localRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        }

        private void FixedUpdate()
        {
            if (GameManager.IsPaused) return;

            HookAimPoint();
            HookingAction();
        }

        private void HookAimPoint()
        {
            var hit = Physics2D.Raycast(Player.HookAnchor.position, Player.HookAnchor.right, detectionRange,
                aimHookLayerMask);

            if (hit)
                hookPoint.position = hit.point;
            else
                hookPoint.position = Player.HookAnchor.position + Player.HookAnchor.right * detectionRange;
        }

        private void HookingAction()
        {
            var socketInRange = SelectSocketInRange();

            if (socketInRange != null)
            {
                selectedHookDisplay.position = socketInRange.transform.position;
                selectedHookDisplay.gameObject.SetActive(true);
                if (CantHook()) return;
                StartCoroutine(HookPulling(socketInRange));
            }
            else
            {
                selectedHookDisplay.gameObject.SetActive(false);
                if (CantHook()) return;
                StartCoroutine(FailedHook());
            }
        }

        private bool CantHook() => !InputReader.Hook || !(hookTime <= 0f) || Player.IsHooking;

        private IEnumerator FailedHook()
        {
            var elapsedTime = 0f;
            hookingDisplay.ActivateRope();
            Player.Hooking(true);

            while (InputReader.Hook && elapsedTime < 0.15f)
            {
                hookingDisplay.DrawRopeWaves(hookPoint.position);
                elapsedTime += Time.unscaledDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            Player.Hooking(false);
            hookTime = hookRate;
            hookingDisplay.StopRope();
        }

        private IEnumerator HookPulling(HookSocket socket)
        {
            target = socket.transform.position;
            Player.Hooking(true);
            Rigidbody.gravityScale = 0f;
            Rigidbody.velocity = Vector2.zero;
            hookingDisplay.ActivateRope();

            var lastDistance = Vector3.Distance(target, transform.position);
            var currentDistance = lastDistance;
            var direction = (target - transform.position).normalized;

            while (currentDistance <= lastDistance && InputReader.Hook && socket.State)
            {
                hookingDisplay.DrawRopeWaves(target);
                Rigidbody.velocity = direction * hookSpeed;
                lastDistance = currentDistance;
                yield return new WaitForFixedUpdate();
                currentDistance = Vector3.Distance(target, transform.position);
            }

            hookingDisplay.StopRope();
            target = Vector3.zero;
            Player.Hooking(false);
            hookTime = hookRate;
            Rigidbody.gravityScale = Player.InitialGravity;
            Rigidbody.velocity = Rigidbody.velocity.normalized * hookResidualSpeed;
        }

        private HookSocket SelectSocketInRange()
        {
            var results = Physics2D.OverlapCircleNonAlloc(Player.HookAnchor.position, detectionRange, collisions);

            var minAngle = float.MaxValue;
            HookSocket closestSocket = null;

            for (int i = 0; i < results; i++)
            {
                if (!collisions[i].TryGetComponent(out HookSocket socket)) continue;
                if (!socket.State) continue;

                var position = transform.position;
                var hookDirection = GetHookDirection();
                var distance = (socket.transform.position - position).magnitude;
                var socketDirection = (socket.transform.position - position).normalized;

                var angle = Vector2.Angle(hookDirection, socketDirection);

                if (!(angle < toleranceAngle) || !(angle < minAngle)) continue;
                if (Physics2D.Linecast(Player.HookAnchor.position, socket.transform.position, layerMask)) continue;

                closestSocket = socket;
                minAngle = angle;
            }

            return closestSocket;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            var offset = Vector3.up * 0.5f;
            Gizmos.DrawWireSphere(transform.position + offset, detectionRange);
        }
    }
}