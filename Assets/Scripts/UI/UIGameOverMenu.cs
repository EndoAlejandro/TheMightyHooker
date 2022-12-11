using TMPro;
using UnityEngine;

namespace UI
{
    public class UIGameOverMenu : MonoBehaviour
    {
        [SerializeField] private TMP_Text deathCounter;
        [SerializeField] private TMP_Text timeCounter;

        private void Start()
        {
            var metrics = GameManager.Instance.PlayerMetrics;
            deathCounter.SetText($"X {metrics.DeathCount}");
            timeCounter.SetText(metrics.GetTimeWithFormat());
        }

        public void GoHome() => GameManager.Instance.ReturnToMainMenu();
    }
}