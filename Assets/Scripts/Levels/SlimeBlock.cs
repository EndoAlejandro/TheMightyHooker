using PlayerComponents;
using UnityEngine;

namespace Levels
{
    public class SlimeBlock : MonoBehaviour
    {
        [SerializeField] private float normalTolerance = 0.75f;
        
        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.contacts[0].normal.y > -normalTolerance) return;
            if (col.transform.TryGetComponent(out Player player))
                player.SlimeBlock();
        }
    }
}