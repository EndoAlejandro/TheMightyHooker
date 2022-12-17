using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIMainMenu : MonoBehaviour
    {
        [SerializeField] private Button continueButton;

        private void Awake()
        {
            if (GameManager.Instance == null)
                GameManager.CreateGameManager();

            continueButton.interactable = GameManager.Instance.CurrentProgress != Vector2Int.zero;
        }

        public void NewGame() => GameManager.Instance.LoadNewGame();
        public void Continue() => GameManager.Instance.LoadContinueGame();
        public void Credits() => GameManager.Instance.Credits();
        public void GoHome() => GameManager.Instance.ReturnToMainMenu();
        public void Options() => GameManager.Instance.Options();
        public void Exit() => GameManager.Instance.ExitGame();
    }
}