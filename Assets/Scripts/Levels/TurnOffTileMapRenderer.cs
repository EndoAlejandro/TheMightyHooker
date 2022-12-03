using UnityEngine;
using UnityEngine.Tilemaps;

namespace Levels
{
    public class TurnOffTileMapRenderer : MonoBehaviour
    {
        private void Awake() => GetComponent<TilemapRenderer>().enabled = false;
    }
}