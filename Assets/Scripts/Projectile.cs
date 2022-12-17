using System.Collections;
using Enemies;
using Pooling;
using UnityEngine;

public class Projectile : PooledMonoBehaviour
{
    [SerializeField] private ShotHitFx collisionFxPrefab;

    private new Rigidbody2D rigidbody;
    private void Awake() => rigidbody = GetComponent<Rigidbody2D>();

    public void Initialize(Vector3 direction, float speed, float lifeTime)
    {
        var t = transform;
        t.right = direction;
        rigidbody.velocity = t.right * speed;
        StartCoroutine(LifeTimeCountDown(lifeTime));
    }

    public void Initialize(Vector3 direction, float speed)
    {
        var t = transform;
        t.right = direction;
        rigidbody.velocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out IDie die)) die.Die();
        StopAllCoroutines();
        DestroyProjectile();
    }

    private IEnumerator LifeTimeCountDown(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        DestroyProjectile();
    }

    private void DestroyProjectile()
    {
        var fx = collisionFxPrefab.Get<ShotHitFx>(transform.position, Quaternion.identity);
        fx.PlayAnimation();
        ReturnToPool();
    }

    protected override void OnDisable()
    {
        rigidbody.velocity = Vector2.zero;
        base.OnDisable();
    }
}