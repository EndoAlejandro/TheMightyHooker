using UnityEngine;

namespace UI
{
    public class UIOptions : MonoBehaviour
    {
        [SerializeField] private UIVolumeSlider masterVolume;
        [SerializeField] private UIVolumeSlider musicVolume;
        [SerializeField] private UIVolumeSlider fxVolume;

        private void Start()
        {
            masterVolume.SetInitialVolume(SaveSystem.GetVolume(SaveSystem.PrefsField.Master));
            musicVolume.SetInitialVolume(SaveSystem.GetVolume(SaveSystem.PrefsField.Music));
            fxVolume.SetInitialVolume(SaveSystem.GetVolume(SaveSystem.PrefsField.Fx));
        }

        public void HomeButtonPressed()
        {
            SaveSystem.SetVolume(SaveSystem.PrefsField.Master, masterVolume.Volume);
            SaveSystem.SetVolume(SaveSystem.PrefsField.Music, musicVolume.Volume);
            SaveSystem.SetVolume(SaveSystem.PrefsField.Fx, fxVolume.Volume);
            GameManager.Instance.ReturnToMainMenu();
        }
    }
}