using UnityEngine;

namespace UI
{
    public class UIMainMenu : MonoBehaviour
    {
        private void Awake()
        {
            if (GameManager.Instance == null)
                GameManager.CreateGameManager();
        }

        public void Play() => GameManager.Instance.LoadGameScene();
        public void Credits() => Debug.Log("Feature pending");
        public void GoHome() => GameManager.Instance.ReturnToMainMenu();
        public void Options() => GameManager.Instance.Options();
        public void Exit() => GameManager.Instance.ExitGame();
    }
}