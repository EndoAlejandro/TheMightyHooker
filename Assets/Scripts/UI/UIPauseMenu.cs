using CustomUtils;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIPauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject container;
        [SerializeField] private TMP_Text progressText;

        private void Awake()
        {
            container.SetActive(false);
            GameManager.Instance.OnPause += OnPause;
        }

        private void Start() => progressText.SetText(Utils.ProgressFormat(GameManager.Instance.CurrentProgress));
        private void OnPause(bool isPaused) => container.SetActive(isPaused);
        public void OnContinuePressed() => GameManager.Instance.UnPauseGame();
        public void OnHomePressed() => GameManager.Instance.ReturnToMainMenu();

        private void OnDestroy()
        {
            if (GameManager.Instance == null) return;
            GameManager.Instance.OnPause -= OnPause;
        }
    }
}