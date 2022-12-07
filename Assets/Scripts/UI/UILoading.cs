using UnityEngine;

namespace UI
{
    public class UILoading : MonoBehaviour
    {
        private void Awake()
        {
            if (GameManager.Instance == null)
                GameManager.CreateGameManager();
        }
    }
}