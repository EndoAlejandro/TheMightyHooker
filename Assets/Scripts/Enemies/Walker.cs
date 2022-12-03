using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(EnvironmentCheck))]
    public class Walker : Enemy
    {
        [SerializeField] private float speed;

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
            Grounded = environmentCheck.Grounded;

            if (!Grounded)
            {
                Rigidbody.velocity = new Vector2(0f, Rigidbody.velocity.y);
                return;
            }
            
            Movement();
        }

        private void Movement()
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
    }
}