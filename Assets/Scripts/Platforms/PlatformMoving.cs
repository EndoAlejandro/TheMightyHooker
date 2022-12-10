using PlayerComponents;
using UnityEngine;

namespace Platforms
{
    public class PlatformMoving : MonoBehaviour
    {
        [Header("Display")] [SerializeField] private Sprite movingPlatform;
        [SerializeField] private Sprite stillPlatform;

        [Header("Movement Points")] [SerializeField]
        private Transform body;

        [SerializeField] private Transform startPoint;
        [SerializeField] private Transform endPoint;

        [Header("Timers")] [SerializeField] private float speed;
        [SerializeField] private float stillTime;

        [Range(0f, 1f)] [SerializeField] private float normalTolerance = 0.25f;

        private SpriteRenderer[] renderers;

        private Vector3 direction;
        private Vector3 target;
        private bool goingToEndPoint;

        private float distance;
        private float lastDistance;
        private float currentStillTime;

        private void Awake() => renderers = body.GetComponentsInChildren<SpriteRenderer>();

        private void Start()
        {
            target = !goingToEndPoint ? startPoint.position : endPoint.position;
            SwitchDirection();
        }

        private void Update()
        {
            currentStillTime -= Time.deltaTime;

            if (currentStillTime < 0)
                Movement();
        }

        private float GetDistance() => Vector3.Distance(target, body.transform.position);

        private void Movement()
        {
            lastDistance = distance;
            body.Translate(direction * (speed * Time.deltaTime));
            distance = GetDistance();

            if (lastDistance < distance) SwitchDirection();
        }

        private void SwitchDirection()
        {
            body.transform.position = target;

            goingToEndPoint = !goingToEndPoint;
            currentStillTime = stillTime;

            target = goingToEndPoint ? endPoint.position : startPoint.position;
            direction = (target - body.transform.position).normalized;

            distance = GetDistance();
        }

        private void ChangeSprites(bool moving)
        {
            foreach (var spriteRenderer in renderers)
            {
                spriteRenderer.sprite = moving ? movingPlatform : stillPlatform;
            }
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (!col.transform.TryGetComponent(out Player player)) return;

            var perpendicularity = Vector2.Dot(col.contacts[0].normal, Vector2.down);

            if (perpendicularity >= normalTolerance)
                player.transform.SetParent(body);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out Player player))
                player.Die();
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.transform.parent == body)
                other.transform.parent = null;
        }
    }
}