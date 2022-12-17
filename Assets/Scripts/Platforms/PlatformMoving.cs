using UnityEngine;

namespace Platforms
{
    public abstract class PlatformMoving : MonoBehaviour
    {
        [Header("Display")] [SerializeField] protected Sprite movingPlatform;
        [SerializeField] protected Sprite stillPlatform;

        [Header("Movement Points")] [SerializeField]
        protected Transform body;

        [SerializeField] protected Transform startPoint;
        [SerializeField] protected Transform endPoint;
        [Range(0f, 1f)] [SerializeField] protected float normalTolerance = 0.25f;

        [Header("Timers")] [SerializeField] protected float speed;
        [SerializeField] protected float stillTime;

        private SpriteRenderer[] renderers;

        protected Vector3 Direction;
        protected Vector3 Target;

        protected bool GoingToEndPoint;
        protected bool CanMove;

        protected bool EndReached => LastDistance < Distance;
        
        protected float Distance;
        protected float LastDistance;
        protected float CurrentStillTime;

        protected virtual void Awake() => renderers = body.GetComponentsInChildren<SpriteRenderer>();
        protected abstract void Movement();

        protected void ChangeSprites(bool moving)
        {
            foreach (var spriteRenderer in renderers)
                spriteRenderer.sprite = moving ? movingPlatform : stillPlatform;
        }
        
        protected float GetDistance() => Vector3.Distance(Target, body.transform.position);
    }
}