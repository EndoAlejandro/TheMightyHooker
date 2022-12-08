using UnityEngine;

namespace UI
{
    public class UIAudio : MonoBehaviour
    {
        public void OnSelected() => SoundManager.Instance.PlayNavigate();
        public void OnPressed() => SoundManager.Instance.PlaySubmit();
    }
}
