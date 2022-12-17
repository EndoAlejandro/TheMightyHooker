using UnityEngine;

namespace UI
{
    public class UICredits : MonoBehaviour
    {
        public void HomeButtonPressed() => GameManager.Instance.ReturnToMainMenu();
    }
}