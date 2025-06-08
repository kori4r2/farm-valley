using System.Collections.Generic;
using Toblerone.Toolbox;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FarmValley {
    public class PlayerController : MonoBehaviour {
        [Header("Movement")]
        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private float moveSpeed;
        [SerializeField] private Movable2D movable;
        private Vector2 moveInput = Vector2.zero;

        [Header("Animation")]
        [SerializeField] private MoveAnimator moveAnimator;

        [Header("Interactions")]
        [SerializeField] private List<PlayerInput> inputs;

        [Header("References")]
        [SerializeField] private Player playerReference;
        [SerializeField] private BoolVariable isPaused;
        private VariableObserver<bool> pauseObserver;

        private void Awake() {
            pauseObserver = new VariableObserver<bool>(isPaused, OnPauseChanged);
            playerReference.SetPlayer(this);
            moveAnimator.Init();
        }

        private void OnPauseChanged(bool paused) {
            if (paused)
                movable.BlockMovement();
            else
                movable.AllowDynamicMovement();
        }

        private void OnDestroy() {
            playerReference.UnsetPlayer(this);
        }

        private void OnEnable() {
            AddInputCallbacks();
            pauseObserver.StartWatching();
        }

        private void AddInputCallbacks() {
            moveAction.action.performed += OnMove;
            moveAction.action.canceled += OnMove;
            foreach (PlayerInput playerInput in inputs) {
                playerInput.SetupCallbacks();
            }
        }

        private void OnDisable() {
            RemoveInputCallbacks();
            pauseObserver.StopWatching();
        }

        private void RemoveInputCallbacks() {
            moveAction.action.performed -= OnMove;
            moveAction.action.canceled -= OnMove;
            foreach (PlayerInput playerInput in inputs) {
                playerInput.RemoveCallbacks();
            }
        }

        private void OnMove(InputAction.CallbackContext context) {
            moveInput = context.ReadValue<Vector2>();
        }

        private void Update() {
            if (isPaused.Value) return;

            movable.SetVelocity(moveSpeed * moveInput);
            moveAnimator.Update(movable.CurrentVelocity);
        }

        private void FixedUpdate() {
            movable.UpdateMovable();
        }
    }
}