using Toblerone.Toolbox;
using UnityEngine;
using UnityEngine.Events;

namespace FarmValley {
    [CreateAssetMenu(menuName = "FarmValley/UI/InventoryDragItem")]
    public class InventoryDragItem : ScriptableObject {
        private readonly UnityEvent<Item> draggedItemChanged = new();
        private InventorySlot draggedSlot;
        private InventorySlot targetSlot;

        public bool IsDraggingSomething => draggedSlot != null;

        public void Reset() {
            draggedSlot = null;
            targetSlot = null;
            draggedItemChanged.RemoveAllListeners();
        }

        public void ObserveDragChanges(UnityAction<Item> callback) {
            draggedItemChanged.AddListener(callback);
        }

        public void StopObservingDragChanges(UnityAction<Item> callback) {
            draggedItemChanged.RemoveListener(callback);
        }

        public bool IsBeingDragged(InventorySlot slot) {
            return draggedSlot != null && draggedSlot == slot;
        }

        public void BeginDrag(InventorySlot slot) {
            draggedSlot = slot;
            targetSlot = slot;
            draggedItemChanged.Invoke(slot?.Item);
        }

        public void RemoveIfTarget(InventorySlot slot) {
            if (targetSlot == slot) targetSlot = null;
        }

        public void SetTarget(InventorySlot slot) {
            targetSlot = slot;
        }

        public void FinishDrag(StringVariable description) {
            draggedItemChanged.Invoke(null);
            if (draggedSlot == null || targetSlot == null) {
                ShowNewTargetDescription(description);
                return;
            }

            if (targetSlot == draggedSlot) {
                ShowNewTargetDescription(description);
                draggedSlot = null;
                targetSlot = null;
                return;
            }

            targetSlot.SwapContents(draggedSlot);
            ShowNewTargetDescription(description);
            draggedSlot = null;
            targetSlot = null;
        }

        private void ShowNewTargetDescription(StringVariable description) {
            description.Value = targetSlot != null && !targetSlot.IsEmpty ?
                targetSlot.Item.Description :
                string.Empty;
        }
    }
}