using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace FarmValley {
    public class InventoryDragItemView : MonoBehaviour {
        [SerializeField] private InventoryDragItem reference;
        [SerializeField] private Image dragIcon;
        [SerializeField] private InputActionReference pointerMove;

        private void Awake() {
            reference.Reset();
            reference.ObserveDragChanges(OnItemDraggedChanged);
            dragIcon.gameObject.SetActive(false);
        }

        private void OnEnable() {
            pointerMove.action.performed += OnPointerMove;
        }

        private void OnDisable() {
            pointerMove.action.performed -= OnPointerMove;
        }

        private void OnDestroy() {
            reference.StopObservingDragChanges(OnItemDraggedChanged);
        }

        private void OnItemDraggedChanged(Item item) {
            if (item is null || !item) {
                dragIcon.gameObject.SetActive(false);
                return;
            }

            dragIcon.gameObject.SetActive(true);
            dragIcon.sprite = item.Icon;
        }

        private void OnPointerMove(InputAction.CallbackContext context) {
            Vector2 screenSize = new(Screen.width, Screen.height);
            Vector2 screenPosition = ClampVector2(context.ReadValue<Vector2>(), Vector2.zero, screenSize);
            dragIcon.transform.position = new Vector3(screenPosition.x, screenPosition.y, transform.position.z);
        }

        private static Vector2 ClampVector2(Vector2 value, Vector2 minValue, Vector2 maxValue) {
            return new Vector2(Mathf.Clamp(value.x, minValue.x, maxValue.x),
                               Mathf.Clamp(value.y, minValue.y, maxValue.y));
        }
    }
}