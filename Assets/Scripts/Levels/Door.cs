using System;
using PlayerComponents;
using UnityEngine;

namespace Levels
{
    public class Door : MonoBehaviour
    {
        private static readonly int IsOn1 = Animator.StringToHash("IsOn");
        private Animator animator;
        private LevelsManager levelsManager;
        public bool IsOn { get; private set; }

        private void Awake() => animator = GetComponentInChildren<Animator>();

        public void TurnOn()
        {
            IsOn = true;
            animator.SetBool(IsOn1, true);
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            if (!IsOn || !col.TryGetComponent(out Player player)) return;
            
            IsOn = false;
            levelsManager.WinLevel();
        }

        public void AssignManager(LevelsManager levelsManager) => this.levelsManager = levelsManager;

        public void TurnOff()
        {
            IsOn = false;
            animator.SetBool(IsOn1, false);
        }
    }
}