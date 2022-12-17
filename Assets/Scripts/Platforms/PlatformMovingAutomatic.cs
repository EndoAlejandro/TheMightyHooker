using PlayerComponents;
using UnityEngine;

namespace Platforms
{
    public class PlatformMovingAutomatic : PlatformMoving
    {
        protected virtual void Start()
        {
            Target = !GoingToEndPoint ? startPoint.position : endPoint.position;
            SwitchDirection();
        }

        protected virtual void Update()
        {
            if (!CanMove)
                CurrentStillTime -= Time.deltaTime;

            if (CurrentStillTime < 0 && !CanMove)
            {
                CanMove = true;
                ChangeSprites(true);
            }

            if (!CanMove) return;
            Movement();

            if (!EndReached) return;
            body.transform.position = Target;
            SwitchDirection();
        }


        protected override void Movement()
        {
            LastDistance = Distance;
            body.Translate(Direction * (speed * Time.deltaTime));
            Distance = GetDistance();
        }

        private void SwitchDirection()
        {
            ChangeSprites(false);
            
            CanMove = false;

            GoingToEndPoint = !GoingToEndPoint;
            CurrentStillTime = stillTime;

            Target = GoingToEndPoint ? endPoint.position : startPoint.position;
            Direction = (Target - body.transform.position).normalized;

            Distance = GetDistance();
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