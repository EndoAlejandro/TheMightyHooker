using Enemies;
using UnityEngine;

namespace PlayerComponents
{
    [RequireComponent(typeof(Player))]
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class PlayerComponent : MonoBehaviour
    {
        protected Player Player;
        protected PlayerInput Input;
        protected Rigidbody2D Rigidbody;
        protected Hook Hook;

        protected virtual void Awake()
        {
            Player = GetComponent<Player>();
            Input = GetComponent<PlayerInput>();
            Rigidbody = GetComponent<Rigidbody2D>();
            Hook = GetComponentInChildren<Hook>();
        }

        protected Vector3 GetHookDirection() => (Hook.transform.position - Player.HookAnchor.position).normalized;
    }
}