using System;
using PlayerComponents;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class Enemy : MonoBehaviour, IDie
    {
        public event Action OnDeath;
        public Rigidbody2D Rigidbody { get; private set; }
        public bool IsFacingRight { get; protected set; }
        public bool Grounded { get; protected set; }
        public bool IsAlive { get; protected set; }

        protected virtual void Awake() => Rigidbody = GetComponent<Rigidbody2D>();
        protected virtual void Start() => IsAlive = true;

        protected virtual void OnCollisionEnter2D(Collision2D col)
        {
            if (col.transform.TryGetComponent(out Player player))
                player.Die();
        }

        public virtual void Die() => OnDeath?.Invoke();
    }
}