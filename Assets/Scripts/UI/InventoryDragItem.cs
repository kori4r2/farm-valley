using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace FarmValley {
    [CreateAssetMenu(menuName = "FarmValley/UI/InventoryDragItem")]
    public class InventoryDragItem : ScriptableObject {
        private InventorySlot draggedSlot = null;
        private InventorySlot targetSlot = null;
        private readonly UnityEvent<Item> draggedItemChanged = new UnityEvent<Item>();

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

        public bool IsDraggingSomething => draggedSlot != null;

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

        public void FinishDrag() {
            draggedItemChanged.Invoke(null);
            if (draggedSlot == null || targetSlot == null) return;

            if (targetSlot == draggedSlot) {
                draggedSlot = null;
                targetSlot = null;
                return;
            }

            targetSlot.SwapContents(draggedSlot);
            draggedSlot = null;
            targetSlot = null;
        }
    }
}