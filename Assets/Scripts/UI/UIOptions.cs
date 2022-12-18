using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIOptions : MonoBehaviour
    {
        [SerializeField] private UIVolumeSlider masterVolume;
        [SerializeField] private UIVolumeSlider musicVolume;
        [SerializeField] private UIVolumeSlider fxVolume;

        [SerializeField] private Button assistModeButton;
        private TMP_Text assistModeStatusText;
        private bool assistMode;
        private void Awake() => assistModeStatusText = assistModeButton.GetComponentInChildren<TMP_Text>();

        private void Start()
        {
            masterVolume.SetInitialVolume(SaveSystem.GetVolume(SaveSystem.PrefsField.Master));
            musicVolume.SetInitialVolume(SaveSystem.GetVolume(SaveSystem.PrefsField.Music));
            fxVolume.SetInitialVolume(SaveSystem.GetVolume(SaveSystem.PrefsField.Fx));

            assistMode = SaveSystem.GetAssistMode();
            SetAssistModeState();
        }

        public void HomeButtonPressed()
        {
            SaveSystem.SetVolume(SaveSystem.PrefsField.Master, masterVolume.Volume);
            SaveSystem.SetVolume(SaveSystem.PrefsField.Music, musicVolume.Volume);
            SaveSystem.SetVolume(SaveSystem.PrefsField.Fx, fxVolume.Volume);
            SaveSystem.SetAssistMode(assistMode);
            GameManager.Instance.ReturnToMainMenu();
        }

        public void AssistModeButtonPressed()
        {
            assistMode = !assistMode;
            SetAssistModeState();
        }

        private void SetAssistModeState()
        {
            assistModeStatusText.color = assistMode ? Color.white : Color.black;
            assistModeStatusText.SetText(assistMode ? "On" : "Off");
        }
    }
}