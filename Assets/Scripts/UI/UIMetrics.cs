using TMPro;
using UnityEngine;

namespace UI
{
    public class UIMetrics : MonoBehaviour
    {
        [SerializeField] private TMP_Text timer;
        [SerializeField] private TMP_Text deathCounter;

        private PlayerMetrics playerMetrics;

        private string currentTime;

        private void Awake() => GameManager.Instance.OnDeath += OnDeath;

        private void Start()
        {
            playerMetrics = GameManager.Instance.PlayerMetrics;
            OnDeath(playerMetrics.DeathCount);
        }

        private void OnDeath(int deathCount) => deathCounter.SetText("X " + deathCount);

        private void Update()
        {
            if (playerMetrics.GetTimeWithFormat().Equals(currentTime)) return;

            currentTime = playerMetrics.GetTimeWithFormat();
            timer.SetText(currentTime);
        }
    }
}