using PlayerComponents;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class LocomotionDeprecated : MonoBehaviour
{
    // Fields related with the basic locomotion of the character.
    [Header("Movement")] [SerializeField] private float speed = 1f;
    [SerializeField] private float jumpHeight = 2f;

    // Fields related with the ladders.
    [Header("Ladders")] [SerializeField] private float distance;
    [SerializeField] private LayerMask ladderLayerMask;

    private float initialGravityScale;

    private RaycastHit2D hit;

    // Read input from another script so it can be changed easily.
    private PlayerInput input;

    private EnvironmentCheck environmentCheck;
    private new Collider2D collider;

    private Rigidbody2D rb;

    public bool IsClimbing { get; private set; }
    public bool Grounded { get; private set; }

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        environmentCheck = GetComponent<EnvironmentCheck>();

        collider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        initialGravityScale = rb.gravityScale;
        // The ladder raycast distance is the height of the character.
        distance = collider.bounds.size.y;
    }

    private void Update()
    {
        Grounded = environmentCheck.Grounded;

        ClimbLadder();
        Walk();

        if (input.Jump && Grounded && !IsClimbing)
            Jump();
    }

    private void ClimbLadder()
    {
        // Checks if its hitting a ladder.
        hit = Physics2D.Raycast(transform.position, Vector2.up, distance, ladderLayerMask);

        if (hit.collider != null && hit.transform.CompareTag("Ladder"))
        {
            // If player press up, start climbing.
            if (input.Movement.y > 0)
                IsClimbing = true;
        }
        else
            IsClimbing = false;

        // When climbing, turn off the player gravity and sets his vertical velocity.
        if (IsClimbing && hit.collider != null)
        {
            rb.velocity = new Vector2(rb.velocity.x, input.Movement.y * speed);
            rb.gravityScale = 0f;
        }
        else
            rb.gravityScale = initialGravityScale;
    }

    // Basic Jump.
    private void Jump() => rb.velocity = Vector2.up * jumpHeight;

    // Basic Walk.
    private void Walk() => rb.velocity = new Vector2(input.Movement.x * speed, rb.velocity.y);
}