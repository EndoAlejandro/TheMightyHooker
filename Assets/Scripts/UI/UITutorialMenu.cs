using PlayerComponents;
using UnityEngine;

namespace UI
{
    public class UITutorialMenu : MonoBehaviour
    {
        private const string KeyboardId = "Keyboard&Mouse";
        private const string GamepadId = "Gamepad";

        [SerializeField] private GameObject display;

        [SerializeField] private GameObject keyboardImage;
        [SerializeField] private GameObject gamepadImage;

        private void Awake() => TurnOffDisplay();

        private void Start()
        {
            if (GameManager.Instance.CurrentProgress == Vector2Int.zero)
                FindObjectOfType<TutorialController>().Setup(this);
        }

        public void DisplayTutorial()
        {
            display.SetActive(true);
            keyboardImage.SetActive(PlayerInputReader.PlayerInput.currentControlScheme.Equals(KeyboardId));
            gamepadImage.SetActive(PlayerInputReader.PlayerInput.currentControlScheme.Equals(GamepadId));
        }

        public void TurnOffDisplay() => display.SetActive(false);
    }
}