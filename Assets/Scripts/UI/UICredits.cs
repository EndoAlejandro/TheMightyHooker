using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UICredits : MonoBehaviour
    {
        [SerializeField] private float speed = 0.5f;

        private ScrollRect scrollRect;

        private float currentPosition;

        private void Awake() => scrollRect = GetComponentInChildren<ScrollRect>();

        private void Start() => currentPosition = 1;

        private void Update()
        {
            scrollRect.verticalNormalizedPosition = currentPosition;
            if (currentPosition > 0)
                currentPosition -= Time.deltaTime * speed;
            else if (currentPosition < 0) currentPosition = 0;
            Debug.Log(scrollRect.verticalNormalizedPosition);
        }

        public void HomeButtonPressed() => GameManager.Instance.ReturnToMainMenu();
    }
}