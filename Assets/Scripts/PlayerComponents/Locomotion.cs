using UnityEngine;

namespace PlayerComponents
{
    public class Locomotion : PlayerComponent
    {
        [Header("Movement")] [SerializeField] private float speed = 9f;
        [SerializeField] private float deceleration = 16f;
        [SerializeField] private float acceleration = 13f;
        [SerializeField] private float velocityPower = 0.96f;
        [SerializeField] private float frictionAmount = 0.22f;

        [Header("Jump")] [SerializeField] private float jumpForce = 13f;
        [SerializeField] private float jumpBufferTime = 0.5f;
        [SerializeField] private float coyoteTime = 0.15f;
        [SerializeField] private float fallSpeedLimit = 12f;
        [SerializeField] private float jumpCancellationScale = 1.5f;

        [Header("WallJump")] [SerializeField] private float horizontalJumpForce = 100f;
        [SerializeField] private float wallFallSpeedLimit = 5f;

        private bool isJumping;
        private bool wasGrounded;

        private float lastGroundedTime;
        private float lastJumpTime;

        private float maxFallSpeed;

        private void FixedUpdate()
        {
            if (GameManager.IsPaused || !Player.IsAlive) return;

            Timers();
            Grounding();

            if (!Player.IsHooking)
                Run();

            Friction();

            if (Mathf.Abs(Input.Movement.x) > 0.1f || Player.IsGrounded || !Player.IsTouchingWall)
            {
                Player.IsSliding = Player.IsTouchingWall &&
                                   !Player.IsGrounded &&
                                   Rigidbody.velocity.y < 0;
            }


            if (Input.Jump)
            {
                if (Player.IsSliding)
                    WallJump();
                else if (lastGroundedTime > 0 && !isJumping)
                    Jump();
            }


            if (!Input.Jump && isJumping && Rigidbody.velocity.y > 0)
                JumpCancellation();

            LimitFallingSpeed();
        }

        private void WallJump()
        {
            var lookDirection = Player.IsFacingRight ? -horizontalJumpForce : horizontalJumpForce;
            Rigidbody.velocity = new Vector2(lookDirection, jumpForce);
            Player.Jump();
        }

        private void JumpCancellation()
        {
            var speedReduction = Mathf.Lerp(Rigidbody.velocity.y, 0f, Time.deltaTime * jumpCancellationScale);
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, speedReduction);
        }

        private void Grounding()
        {
            if (Player.IsGrounded)
            {
                if (!wasGrounded) Player.Land();

                lastGroundedTime = coyoteTime;
                isJumping = false;
            }

            wasGrounded = Player.IsGrounded;
        }

        private void LimitFallingSpeed()
        {
            var speedLimit = Player.IsSliding ? wallFallSpeedLimit : fallSpeedLimit;
            if (Rigidbody.velocity.y < -speedLimit && Rigidbody.velocity.y < 0)
                Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, -speedLimit);
        }

        private void Timers()
        {
            lastGroundedTime -= Time.fixedDeltaTime;
            lastJumpTime -= Time.fixedDeltaTime;
        }

        private void Jump()
        {
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, 0f);
            Rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            lastJumpTime = jumpBufferTime;
            lastGroundedTime = 0;
            isJumping = true;

            Player.Jump();
        }

        private void Friction()
        {
            if (!Player.IsGrounded || !(Mathf.Abs(Input.Movement.x) < 0.01f)) return;

            var amount = Mathf.Min(Mathf.Abs(Rigidbody.velocity.x), Mathf.Abs(frictionAmount));
            amount *= Mathf.Sign(Rigidbody.velocity.x);
            Rigidbody.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }

        private void Run()
        {
            var targetSpeed = Input.Movement.x * speed;
            var speedDifference = targetSpeed - Rigidbody.velocity.x;
            var accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
            var movement = Mathf.Pow(Mathf.Abs(speedDifference) * accelerationRate, velocityPower) *
                           Mathf.Sign(speedDifference);

            Rigidbody.AddForce(movement * Vector2.right);
        }

        private void OnDestroy()
        {
            if (Player == null) return;
        }
    }
}