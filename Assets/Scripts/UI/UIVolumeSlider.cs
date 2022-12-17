using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI
{
    public class UIVolumeSlider : MonoBehaviour
    {
        [SerializeField] private AudioMixerGroup audioMixer;
        [SerializeField] private Transform sliderBarsContainer;
        [SerializeField] private Image muteButtonImage;

        [SerializeField] private Sprite unMutedImage;
        [SerializeField] private Sprite mutedImage;

        private RectTransform[] sliderBars;

        //private string mixerName;
        public float Volume { get; private set; }

        private float activeHeight;
        private float unActiveHeight;

        private void Awake()
        {
            FillSliderBars();
            activeHeight = sliderBars[0].sizeDelta.y;
            unActiveHeight = sliderBars[0].sizeDelta.x;
            //mixerName = audioMixer.name + "Volume";
        }

        public void SetInitialVolume(float value)
        {
            if (value < 0)
                audioMixer.audioMixer.GetFloat(audioMixer.name + "Volume", out value);
            Volume = Mathf.Min(value, 10f);
            SetAudioVolume();
        }

        private void FillSliderBars()
        {
            var childCount = sliderBarsContainer.childCount;
            sliderBars = new RectTransform[childCount];

            for (var i = 0; i < childCount; i++)
                sliderBars[i] = sliderBarsContainer.GetChild(i).GetComponent<RectTransform>();
        }

        public void IncreaseButtonPressed()
        {
            Volume = Mathf.Min(Volume + 1f, 10f);
            SetAudioVolume();
        }

        public void DecreaseButtonPressed()
        {
            Volume = Mathf.Max(Volume - 1f, 0.0001f);
            SetAudioVolume();
        }

        public void MuteButtonPressed()
        {
            Volume = Volume < 1 ? 10 : 0.0001f;
            SetAudioVolume();
        }

        private void SetAudioVolume()
        {
            SoundManager.SetMixerGroupVolume(audioMixer, Volume);
            //audioMixer.audioMixer.SetFloat(mixerName, SoundManager.FromNormalizedToLog(Volume));
            UpdateSlider();
        }

        private void UpdateSlider()
        {
            muteButtonImage.sprite = Volume < 1 ? mutedImage : unMutedImage;
            for (int i = 0; i < sliderBars.Length; i++)
                sliderBars[i].sizeDelta =
                    new Vector2(unActiveHeight, i + 1 > Volume ? unActiveHeight : activeHeight);
        }
    }
}