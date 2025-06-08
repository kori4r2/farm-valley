using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace FarmValley {
    [Serializable]
    public class PlayerInput {
        [SerializeField] private InputActionReference action;
        [SerializeField] private UnityEvent response;

        public void SetupCallbacks() {
            action.action.performed += InvokeCallback;
        }

        private void InvokeCallback(InputAction.CallbackContext context) {
            response?.Invoke();
        }

        public void RemoveCallbacks() {
            action.action.performed -= InvokeCallback;
        }
    }
}