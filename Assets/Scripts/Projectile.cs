using System;
using System.Collections;
using Enemies;
using Pooling;
using UnityEngine;

public class Projectile : PooledMonoBehaviour
{
    private new Rigidbody2D rigidbody;
    private void Awake() => rigidbody = GetComponent<Rigidbody2D>();

    public void Initialize(Vector3 direction, float speed, float lifeTime)
    {
        var t = transform;
        t.right = direction;
        rigidbody.velocity = t.right * speed;
        StartCoroutine(LifeTimeCountDown(lifeTime));
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out IDie die)) die.Die();
        StopAllCoroutines();
        ReturnToPool();
    }

    private IEnumerator LifeTimeCountDown(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        ReturnToPool();
    }

    protected override void OnDisable()
    {
        rigidbody.velocity = Vector2.zero;
        base.OnDisable();
    }
}