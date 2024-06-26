using System;
using System.Collections;
using Interfaces;
using PlayerComponents;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class Enemy : MonoBehaviour, IDie, IResettable
    {
        public event Action OnDeath;
        public event Action<bool> OnStun;
        public event Action OnSpawn;

        [Header("Stun")] [SerializeField] private float stunTime;
        [SerializeField] private bool canDie;

        public Rigidbody2D Rigidbody { get; private set; }
        private new Collider2D collider;
        private Vector3 initialPosition;

        public bool IsFacingRight { get; protected set; }
        public bool Grounded { get; protected set; }
        public bool IsAlive { get; protected set; }
        protected bool IsStunned { get; private set; }

        protected virtual void Awake()
        {
            collider = GetComponent<Collider2D>();
            Rigidbody = GetComponent<Rigidbody2D>();

            initialPosition = transform.position;
        }

        protected abstract void Movement();

        protected virtual void Start() => IsAlive = true;

        protected virtual void OnCollisionEnter2D(Collision2D col)
        {
            if (col.transform.TryGetComponent(out Player player))
                player.Die();
        }

        public virtual void Die()
        {
            if (canDie)
            {
                IsAlive = false;
                OnDeath?.Invoke();
            }
            else
                SetStunState(true);
        }

        private void SetStunState(bool state)
        {
            IsStunned = state;
            OnStun?.Invoke(state);
            collider.enabled = !state;
            if (state)
            {
                Rigidbody.Sleep();
                StartCoroutine(StunTimer());
            }
            else
                Rigidbody.IsAwake();
        }

        private IEnumerator StunTimer()
        {
            yield return new WaitForSeconds(stunTime);
            SetStunState(false);
        }

        public void Reset()
        {
            SetStunState(false);
            IsAlive = true;
            transform.position = initialPosition;
            gameObject.SetActive(true);
            OnSpawn?.Invoke();
        }
    }
}