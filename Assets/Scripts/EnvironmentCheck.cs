using Hazards;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnvironmentCheck : MonoBehaviour
{
    [Header("Walls")] [SerializeField] private LayerMask wallLayerMask;
    [SerializeField] private float wallCheckRadius = 0.1f;

    private Collider2D[] collisions;

    [Header("Ground")] [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float checkDistance = 0.05f;

    private new Collider2D collider;

    public bool LeftGrounded { get; private set; }
    public bool RightGrounded { get; private set; }
    public bool Grounded => LeftGrounded || RightGrounded;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        collisions = new Collider2D[20];
    }

    private void Update()
    {
        // Sets the value of each foot.
        LeftGrounded = CheckLeftFoot();
        RightGrounded = CheckRightFoot();
    }

    public bool CheckWalls(bool isFacingRight)
    {
        var xOffset = isFacingRight ? collider.bounds.max.x : collider.bounds.min.x;
        var sideToCheck = new Vector3(xOffset, collider.bounds.center.y);

        var results = Physics2D.OverlapCircleNonAlloc(sideToCheck, wallCheckRadius, collisions, wallLayerMask);

        var lastResults = results;
        for (int i = 0; i < results; i++)
        {
            if (collisions[i].TryGetComponent(out Spikes spikes))
                lastResults += spikes.IsActive ? 0 : -1;
        }
        return lastResults > 0;
    }

    private bool CheckRightFoot()
    {
        var origin = new Vector2(collider.bounds.max.x, collider.bounds.min.y);
        return Physics2D.Raycast(origin, Vector2.down, checkDistance, groundLayerMask);
    }

    private bool CheckLeftFoot()
    {
        var origin = new Vector2(collider.bounds.min.x, collider.bounds.min.y);
        return Physics2D.Raycast(origin, Vector2.down, checkDistance, groundLayerMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var col = GetComponent<Collider2D>();
        var leftOrigin = new Vector3(col.bounds.min.x, col.bounds.min.y);
        Gizmos.DrawLine(leftOrigin, leftOrigin + Vector3.down * checkDistance);
        Gizmos.color = Color.yellow;
        var rightOrigin = new Vector3(col.bounds.max.x, col.bounds.min.y);
        Gizmos.DrawLine(rightOrigin, rightOrigin + Vector3.down * checkDistance);
    }
}