using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pooling
{
    public class Pool : MonoBehaviour
    {
        private static Dictionary<PooledMonoBehaviour, Pool> pools = new Dictionary<PooledMonoBehaviour, Pool>();

        private Queue<PooledMonoBehaviour> objects = new Queue<PooledMonoBehaviour>();

        private PooledMonoBehaviour prefab;

        public static Pool GetPool(PooledMonoBehaviour _prefab)
        {
            if (pools.ContainsKey(_prefab)) return pools[_prefab];

            var pool = new GameObject("Pool-" + _prefab.name).AddComponent<Pool>();
            pool.prefab = _prefab;

            pools.Add(_prefab, pool);
            return pool;
        }

        public T Get<T>() where T : PooledMonoBehaviour
        {
            if (objects.Count == 0) GrowPool();

            var pooledObject = objects.Dequeue();
            return pooledObject as T;
        }

        private void GrowPool()
        {
            for (int i = 0; i < prefab.InitialPoolSize; i++)
            {
                var pooledObject = Instantiate(prefab, this.transform, true) as PooledMonoBehaviour;
                pooledObject.name += " " + i;
                pooledObject.OnReturnToPool += AddToQueue;
                pooledObject.gameObject.SetActive(false);
            }
        }

        private void AddToQueue(PooledMonoBehaviour pooledObject)
        {
            pooledObject.transform.SetParent(this.transform);
            objects.Enqueue(pooledObject);
        }

        private void OnDestroy()
        {
            pools.Clear();
            while (objects.Count > 0)
            {
                var o = objects.Dequeue();
                o.OnReturnToPool -= AddToQueue;
                Destroy(o.gameObject);
            }
        }
    }
}