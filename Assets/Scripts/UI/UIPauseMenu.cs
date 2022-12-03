using UnityEngine;

namespace UI
{
    public class UIPauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject container;

        private void Awake()
        {
            container.SetActive(false);

            GameManager.Instance.OnPause += OnPause;
        }

        private void OnPause(bool isPaused)
        {
            container.SetActive(isPaused);
        }

        public void OnContinuePressed() => GameManager.Instance.UnPauseGame();
        public void OnHomePressed() => GameManager.Instance.ReturnToMainMenu();
        
        private void OnDestroy()
        {
            if (GameManager.Instance == null) return;
            GameManager.Instance.OnPause -= OnPause;
        }
    }
}