using PlayerComponents;
using UnityEngine;

namespace Levels
{
    public class Door : MonoBehaviour
    {
        private static readonly int IsOn = Animator.StringToHash("IsOn");
        private bool isOn;
        private Animator animator;

        private void Awake() => animator = GetComponentInChildren<Animator>();

        public void TurnOn()
        {
            isOn = true;
            animator.SetBool(IsOn, true);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (isOn && col.TryGetComponent(out Player player)) 
                LevelsManager.WinLevel();
        }
    }
}