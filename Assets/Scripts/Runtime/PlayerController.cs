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

        [Header("Interactions")]
        [SerializeField] private InputActionReference interactAction;

        [Header("References")]
        [SerializeField] private Player playerReference;
        [SerializeField] private BoolVariable isPaused;
        private VariableObserver<bool> pauseObserver;
        [SerializeField] private Interactor interactor;

        private void Awake() {
            pauseObserver = new VariableObserver<bool>(isPaused, OnPauseChanged);
            playerReference.SetPlayer(this);
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
            interactAction.action.performed += OnInteract;
        }

        private void OnInteract(InputAction.CallbackContext context) {
            if (!movable.CanMove || isPaused.Value) return;

            interactor.AttemptInteraction();
        }

        private void OnDisable() {
            RemoveInputCallbacks();
            pauseObserver.StopWatching();
        }

        private void RemoveInputCallbacks() {
            moveAction.action.performed -= OnMove;
            moveAction.action.canceled -= OnMove;
        }

        private void OnMove(InputAction.CallbackContext context) {
            moveInput = context.ReadValue<Vector2>();
        }

        private void Update() {
            if (isPaused.Value) return;

            movable.SetVelocity(moveSpeed * moveInput);
        }

        private void FixedUpdate() {
            movable.UpdateMovable();
        }
    }
}