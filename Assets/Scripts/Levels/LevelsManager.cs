using System;
using PlayerComponents;
using UnityEngine;

namespace Levels
{
    public class LevelsManager : MonoBehaviour
    {
        [SerializeField] private Player playerPrefab;
        [SerializeField] private LevelCluster[] levelsCluster;

        private int currentLevel;
        private int currentSubLevel;

        private static int _maxClusterLevel;

        private void Start()
        {
            currentLevel = GameManager.Instance.CurrentProgress.x;
            currentSubLevel = GameManager.Instance.CurrentProgress.y;

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
        private void Update() => GameManager.Instance.PlayerMetrics.Tick(Time.deltaTime);
        private void OnApplicationPause(bool pauseStatus) => GameManager.Instance.PauseGame();

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
                GameManager.Instance.PauseGame();
        }
    }

    [Serializable]
    public struct LevelCluster
    {
        public Level[] levels;
    }
}