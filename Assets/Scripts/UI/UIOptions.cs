using UnityEngine;

namespace UI
{
    public class UIOptions : MonoBehaviour
    {
        public void HomeButtonPressed() => GameManager.Instance.ReturnToMainMenu();
    }
}
