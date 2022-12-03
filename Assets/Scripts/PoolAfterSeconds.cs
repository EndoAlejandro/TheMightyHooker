using Pooling;
using UnityEngine;

public class PoolAfterSeconds : PooledMonoBehaviour
{
    [SerializeField] private float delay;
    private void OnEnable() => ReturnToPool(delay);
}