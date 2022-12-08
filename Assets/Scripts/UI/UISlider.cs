using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI
{
    public class UISlider : MonoBehaviour
    {
        [SerializeField] private AudioMixerGroup audioMixer;
        [SerializeField] private Transform sliderBarsContainer;
        [SerializeField] private Image muteButtonImage;

        [SerializeField] private Sprite unMutedImage;
        [SerializeField] private Sprite mutedImage;

        private RectTransform[] sliderBars;

        private string mixerName;
        private float volume;

        private float activeHeight;
        private float unActiveHeight;

        private void Awake()
        {
            FillSliderBars();

            mixerName = audioMixer.name + "Volume";
            audioMixer.audioMixer.GetFloat(mixerName, out var value);
            volume = FromLogToNormalized(value);
        }

        private void Start()
        {
            activeHeight = sliderBars[0].sizeDelta.y;
            unActiveHeight = sliderBars[0].sizeDelta.x;

            UpdateSlider();
        }

        private float FromLogToNormalized(float value) => Mathf.Pow(10, value / 20) * 10;
        private float FromNormalizedToLog(float value) => Mathf.Log10(value / 10) * 20;

        private void FillSliderBars()
        {
            var childCount = sliderBarsContainer.childCount;
            sliderBars = new RectTransform[childCount];

            for (var i = 0; i < childCount; i++)
                sliderBars[i] = sliderBarsContainer.GetChild(i).GetComponent<RectTransform>();
        }

        public void IncreaseButtonPressed()
        {
            volume = Mathf.Min(volume + 1f, 10f);
            SetAudioVolume();
            UpdateSlider();
        }

        public void DecreaseButtonPressed()
        {
            volume = Mathf.Max(volume - 1f, 0.0001f);
            SetAudioVolume();
            UpdateSlider();
        }

        public void MuteButtonPressed()
        {
            volume = volume < 1 ? 10 : 0.0001f;
            SetAudioVolume();
            UpdateSlider();
        }

        private void SetAudioVolume() => audioMixer.audioMixer.SetFloat(mixerName, FromNormalizedToLog(volume));

        private void UpdateSlider()
        {
            muteButtonImage.sprite = volume < 1 ? mutedImage : unMutedImage;
            for (int i = 0; i < sliderBars.Length; i++)
                sliderBars[i].sizeDelta =
                    new Vector2(unActiveHeight, i + 1 > volume ? unActiveHeight : activeHeight);
        }
    }
}