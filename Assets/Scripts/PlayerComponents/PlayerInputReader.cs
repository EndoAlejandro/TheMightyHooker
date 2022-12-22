using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerComponents
{
    public class PlayerInputReader : MonoBehaviour
    {
        public static PlayerInput PlayerInput;

        public Vector2 Movement { get; private set; }

        public bool Jump { get; private set; }
        public bool Hook { get; private set; }
        public bool Shoot { get; private set; }
        public bool Pause { get; private set; }

        private void Awake() => PlayerInput = GetComponent<PlayerInput>();

        private void Update()
        {
            if (Pause && !GameManager.IsPaused) GameManager.Instance.PauseGame();
        }

        public void MoveReader(InputAction.CallbackContext context) => Movement = context.ReadValue<Vector2>();

        public void JumpReader(InputAction.CallbackContext context) =>
            Jump = context.ReadValueAsButton();

        public void HookReader(InputAction.CallbackContext context) =>
            Hook = context.ReadValueAsButton();

        public void ShootReader(InputAction.CallbackContext context) =>
            Shoot = context.ReadValueAsButton();

        public void PauseReader(InputAction.CallbackContext context) =>
            Pause = context.ReadValueAsButton();
    }
}