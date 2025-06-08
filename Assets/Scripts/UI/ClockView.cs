using System;
using TMPro;
using Toblerone.Toolbox;
using UnityEngine;

namespace FarmValley {
    public class ClockView : MonoBehaviour {
        [SerializeField] private FloatVariable timeVariable;
        [SerializeField] private TextMeshProUGUI textField;
        private VariableObserver<float> timeObserver;

        private void Awake() {
            timeObserver = new VariableObserver<float>(timeVariable, UpdateTimer);
        }

        private void OnEnable() {
            timeObserver.StartWatching();
        }

        private void OnDisable() {
            timeObserver.StopWatching();
        }

        private void UpdateTimer(float newTimeSeconds) {
            int totalSeconds = Mathf.FloorToInt(Mathf.Max(0f, newTimeSeconds));
            int minutes = Math.DivRem(totalSeconds, 60, out int seconds);
            textField.text = $"{minutes:00}:{seconds:00}";
        }
    }
}