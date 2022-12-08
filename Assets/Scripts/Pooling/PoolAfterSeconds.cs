using UnityEngine;

namespace Pooling
{
    public class PoolAfterSeconds : PooledMonoBehaviour
    {
        [SerializeField] private float delay;
        private void OnEnable() => ReturnToPool(delay);
    }
}