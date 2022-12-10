using System.Collections;
using PlayerComponents;
using UnityEngine;

namespace Platforms
{
    public class PlatformMoving : MonoBehaviour
    {
        [SerializeField] private Sprite movingPlatform;
        [SerializeField] private Sprite stillPlatform;

        [SerializeField] private Transform body;
        [SerializeField] private Transform startPoint;
        [SerializeField] private Transform endPoint;

        [SerializeField] private float speed;
        [SerializeField] private float stillTime;

        [Range(0f, 1f)] [SerializeField] private float normalTolerance = 0.25f;

        private SpriteRenderer[] renderers;

        private Vector3 direction;
        private Vector3 target;
        private bool goingToStartPoint;

        private float distance;
        private float lastDistance;

        private void Awake() => renderers = body.GetComponentsInChildren<SpriteRenderer>();

        private void Start()
        {
            StartCoroutine(Movement());
        }

        private void ChangeSprites(bool moving)
        {
            foreach (var spriteRenderer in renderers)
            {
                spriteRenderer.sprite = moving ? movingPlatform : stillPlatform;
            }
        }

        private float GetDistance() => Vector3.Distance(target, body.transform.position);

        private IEnumerator Movement()
        {
            ChangeSprites(true);
            target = goingToStartPoint ? startPoint.position : endPoint.position;
            distance = GetDistance();
            direction = (target - body.transform.position).normalized;

            lastDistance = float.MaxValue;
            while (lastDistance > distance)
            {
                lastDistance = distance;
                body.Translate(direction * (speed * Time.deltaTime));
                yield return null;
                distance = GetDistance();
            }

            ChangeSprites(false);
            body.transform.position = target;
            StartCoroutine(SwitchDirection());
        }

        private IEnumerator SwitchDirection()
        {
            goingToStartPoint = !goingToStartPoint;
            yield return new WaitForSeconds(stillTime);
            StartCoroutine(Movement());
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (!col.transform.TryGetComponent(out Player player)) return;

            var perpendicularity = Vector2.Dot(col.contacts[0].normal, Vector2.down);

            if (perpendicularity >= normalTolerance)
                player.transform.SetParent(body);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.transform.parent == body)
                other.transform.parent = null;
        }
    }
}