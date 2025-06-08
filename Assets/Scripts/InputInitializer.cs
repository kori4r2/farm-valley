using System;
using Toblerone.Toolbox;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FarmValley {
    [Serializable]
    public class InputInitializer {
        [SerializeField] private InputActionAsset inputActions;
        [SerializeField] private InputMapSwitcher initialInputSwitcher;

        public void Enable() {
            inputActions.Enable();
            initialInputSwitcher.SwitchToMap();
        }
    }
}