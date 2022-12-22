using UnityEngine;

namespace PlayerComponents
{
    [RequireComponent(typeof(Player))]
    [RequireComponent(typeof(PlayerInputReader))]
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class PlayerComponent : MonoBehaviour
    {
        protected Player Player;
        protected PlayerInputReader InputReader;
        protected Rigidbody2D Rigidbody;
        protected Hook Hook;

        protected virtual void Awake()
        {
            Player = GetComponent<Player>();
            InputReader = GetComponent<PlayerInputReader>();
            Rigidbody = GetComponent<Rigidbody2D>();
            Hook = GetComponentInChildren<Hook>();
        }

        protected Vector3 GetHookDirection() => (Hook.transform.position - Player.HookAnchor.position).normalized;
    }
}