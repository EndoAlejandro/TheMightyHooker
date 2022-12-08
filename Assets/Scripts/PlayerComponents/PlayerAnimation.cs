using CustomUtils;
using Levels;
using Pooling;
using UnityEngine;

namespace PlayerComponents
{
    public class PlayerAnimation : PlayerComponent
    {
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int FallSpeed = Animator.StringToHash("FallSpeed");
        private static readonly int Grounded = Animator.StringToHash("Grounded");
        private static readonly int Hooking1 = Animator.StringToHash("Hooking");
        private static readonly int Death = Animator.StringToHash("Death");
        private static readonly int Sliding = Animator.StringToHash("Sliding");

        [SerializeField] private Transform body;

        [Header("Death")] [SerializeField] private PoolAfterSeconds deathDust;
        [SerializeField] private float deathAnimationSpeed = 1f;
        [SerializeField] private float bulletTime = 0.5f;
        [SerializeField] private Vector2 deathShake = new Vector2(0.5f, 0.05f);

        [Header("Jump")] [SerializeField] private PoolAfterSeconds jumpDust;
        [SerializeField] private PoolAfterSeconds landDust;
        [SerializeField] private Vector2 jumpShake = new Vector2(0.1f, 0.05f);

        [Header("Shoot")] [SerializeField] private Vector2 shootShake = new Vector2(0.1f, 0.025f);

        [Header("Walk")] [SerializeField] private PoolAfterSeconds walkDust;
        [SerializeField] private float walkDustTime = 0.5f;
        private float walkDustCurrentTime;

        private Animator animator;
        private SpriteRenderer spriteRenderer;

        protected override void Awake()
        {
            base.Awake();
            animator = body.GetComponent<Animator>();
            spriteRenderer = body.GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            Player.OnJump += OnJump;
            Player.OnLanding += OnLanding;
            Player.OnDeath += OnDeath;
            Player.OnShooting += OnShooting;
        }

        private void OnShooting() => CameraController.Instance.CamShake(shootShake);

        private void OnDeath()
        {
            CameraController.Instance.CamShake(deathShake);
            deathDust.Get<PoolAfterSeconds>(Player.HookAnchor.position, Quaternion.identity);
            animator.SetTrigger(Death);
            Rigidbody.velocity = Vector2.zero;
            Rigidbody.gravityScale = 0f;
            spriteRenderer.enabled = false;
            StartCoroutine(Utils.DieSequence(deathAnimationSpeed, bulletTime, () =>
            {
                //LevelsManager.LoseLevel();
            }));
        }

        private void OnLanding()
        {
            CameraController.Instance.CamShake(jumpShake);
            landDust.Get<PoolAfterSeconds>(transform.position, Quaternion.identity);
        }

        private void OnJump()
        {
            CameraController.Instance.CamShake(jumpShake);
            jumpDust.Get<PoolAfterSeconds>(transform.position, Quaternion.identity);
        }

        private void Update()
        {
            if (GameManager.IsPaused || !Player.IsAlive) return;

            if (Input.Movement.x != 0) Player.IsFacingRight = Input.Movement.x > 0;

            WalkDust();

            spriteRenderer.flipX = !Player.IsFacingRight;
            animator.SetFloat(Speed, Mathf.Abs(Rigidbody.velocity.x));
            animator.SetFloat(FallSpeed, Mathf.Clamp(Rigidbody.velocity.y, -1f, 1f));
            animator.SetBool(Grounded, Player.IsGrounded);
            animator.SetBool(Hooking1, Player.IsHooking);
            animator.SetBool(Sliding, Player.IsSliding);
        }

        private void WalkDust()
        {
            walkDustCurrentTime -= Time.deltaTime;

            if (!Player.IsGrounded || Input.Movement.x == 0 || !(walkDustCurrentTime < 0)) return;

            walkDustCurrentTime = walkDustTime;
            walkDust.Get<PoolAfterSeconds>(transform.position, Quaternion.identity);
        }

        private void OnDestroy()
        {
            if (Player == null) return;

            Player.OnJump -= OnJump;
            Player.OnLanding -= OnLanding;
        }
    }
}