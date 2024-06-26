﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UICredits : MonoBehaviour
    {
        [SerializeField] private float speed = 0.5f;
        [SerializeField] private float waitTime = 1f;
        private ScrollRect scrollRect;

        private float currentPosition;

        private void Awake() => scrollRect = GetComponentInChildren<ScrollRect>();

        private void Start() => currentPosition = 1;

        private void Update()
        {
            if (waitTime > 0)
            {
                waitTime -= Time.deltaTime;
                return;
            }

            scrollRect.verticalNormalizedPosition = currentPosition;
            if (currentPosition > 0)
                currentPosition -= Time.deltaTime * speed;
            else if (currentPosition < 0) currentPosition = 0;
        }

        public void HomeButtonPressed() => GameManager.Instance.ReturnToMainMenu();
    }
}