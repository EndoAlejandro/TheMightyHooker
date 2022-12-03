using UnityEngine;

namespace Enemies
{
    public class EnemyComponent : MonoBehaviour
    {
        protected Enemy Enemy;
        protected virtual void Awake() => Enemy = GetComponent<Enemy>();
    }
}