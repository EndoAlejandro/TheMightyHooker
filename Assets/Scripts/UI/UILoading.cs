using TMPro;
using UnityEngine;

namespace UI
{
    public class UILoading : MonoBehaviour
    {
        [SerializeField] private TMP_Text progressText;

        private void Awake()
        {
            if (GameManager.Instance == null)
                GameManager.CreateGameManager();

            GameManager.Instance.OnLoadingLevel += OnLoadingLevel;
        }

        private void OnLoadingLevel(Vector2Int progress)
        {
            progressText.SetText($"{progress.x + 1} - {progress.y + 1}");
            progressText.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            if (GameManager.Instance == null) return;
            GameManager.Instance.OnLoadingLevel -= OnLoadingLevel;
        }
    }
}