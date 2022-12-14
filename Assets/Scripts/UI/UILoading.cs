using CustomUtils;
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
            progressText.SetText(Utils.ProgressFormat(progress));
            progressText.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            if (GameManager.Instance == null) return;
            GameManager.Instance.OnLoadingLevel -= OnLoadingLevel;
        }
    }
}