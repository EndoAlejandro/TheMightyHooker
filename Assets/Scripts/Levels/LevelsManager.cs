using System;
using PlayerComponents;
using UnityEngine;
using UnityEngine.Serialization;

namespace Levels
{
    public class LevelsManager : MonoBehaviour
    {
        [SerializeField] private Player playerPrefab;

        // [SerializeField] private Level[] levels;
        [SerializeField] private LevelCluster[] levelsCluster;

        private int currentLevel;
        private int currentSubLevel;

        private static int _maxClusterLevel;

        private void Start()
        {
            currentLevel = GameManager.Instance.CurrentLevel;
            currentSubLevel = GameManager.Instance.CurrentSubLevel;

            if (currentLevel >= levelsCluster.Length)
                GameManager.Instance.WinGame();
            else
            {
                _maxClusterLevel = levelsCluster[currentLevel].levels.Length;
                var level = levelsCluster[currentLevel].levels[currentSubLevel];
                Instantiate(level, transform);
                var player = Instantiate(playerPrefab, level.PlayerSpawnPoint.position, Quaternion.identity);
            }
        }

        public static void WinLevel() => GameManager.Instance.WinLevel(_maxClusterLevel);
        public static void LoseLevel() => GameManager.Instance.LoseLevel();
    }

    [Serializable]
    public struct LevelCluster
    {
        public Level[] levels;
    }
}