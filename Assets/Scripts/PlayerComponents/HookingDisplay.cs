using UnityEngine;

namespace PlayerComponents
{
    public class HookingDisplay : MonoBehaviour
    {
        [SerializeField] private int precision = 40;
        [SerializeField] private float straightenLineSpeed = 5f;
        [SerializeField] private AnimationCurve animationCurve;
        [Range(0.01f, 4f)] [SerializeField] private float startWaveSize = 2f;

        [Range(1f, 50f)] [SerializeField] private float ropeProgressionSpeed = 1f;

        private LineRenderer lineRenderer;

        private float waveSize;

        private void Awake() => lineRenderer = GetComponentInChildren<LineRenderer>();
        private void Start() => lineRenderer.positionCount = precision;

        public void DrawRopeWaves(Vector3 socketPosition)
        {
            if (waveSize > 0)
                waveSize -= Time.deltaTime * straightenLineSpeed;
            lineRenderer.enabled = true;
            var distance = socketPosition - transform.position;

            for (int i = 0; i < precision; i++)
            {
                var delta = i / (precision - 1f);
                var offset = Vector2.Perpendicular(distance).normalized * (animationCurve.Evaluate(delta) * waveSize);
                var targetPosition =
                    Vector2.Lerp(transform.position, socketPosition, delta) +
                    offset;
                var currentPosition = Vector2.Lerp(transform.position, targetPosition, ropeProgressionSpeed);
                lineRenderer.SetPosition(i, currentPosition);
            }
        }

        public void ActivateRope()
        {
            lineRenderer.positionCount = precision;
            waveSize = startWaveSize;
            LinePointsToFirePoint();
            lineRenderer.enabled = true;
        }

        private void LinePointsToFirePoint()
        {
            for (int i = 0; i < precision; i++)
                lineRenderer.SetPosition(i, transform.position);
        }

        public void StopRope() => lineRenderer.enabled = false;
    }
}