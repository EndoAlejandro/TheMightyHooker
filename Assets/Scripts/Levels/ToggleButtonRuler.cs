using System;
using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    public class ToggleButtonRuler : MonoBehaviour
    {
        public enum ButtonType
        {
            None,
            Positive,
            Negative,
        }

        [SerializeField] private bool initialState;

        [SerializeField] private Transform positiveChildrenContainer;
        [SerializeField] private Transform negativeChildrenContainer;

        private Dictionary<ButtonType, List<ToggleButton>> buttonsDictionary = new();

        private void Awake()
        {
            buttonsDictionary.Add(ButtonType.Positive, new List<ToggleButton>());
            buttonsDictionary.Add(ButtonType.Negative, new List<ToggleButton>());

            var positiveButtons = positiveChildrenContainer.GetComponentsInChildren<ToggleButton>();
            FillDictionary(positiveButtons, ButtonType.Positive);

            var negativeButtons = negativeChildrenContainer.GetComponentsInChildren<ToggleButton>();
            FillDictionary(negativeButtons, ButtonType.Negative);
        }

        private void Start() => Toggle(initialState, ButtonType.Positive);

        private void FillDictionary(ToggleButton[] buttons, ButtonType type)
        {
            foreach (var button in buttons)
            {
                buttonsDictionary[type].Add(button);
                button.SetRuler(this, type);
            }
        }

        public void Toggle(bool state, ButtonType buttonType)
        {
            foreach (var button in buttonsDictionary[ButtonType.Positive])
            {
                var value = buttonType == ButtonType.Positive ? state : !state;
                button.SetStateFromParent(value);
            }

            foreach (var button in buttonsDictionary[ButtonType.Negative])
            {
                var value = buttonType == ButtonType.Negative ? state : !state;
                button.SetStateFromParent(value);
            }
        }
    }
}