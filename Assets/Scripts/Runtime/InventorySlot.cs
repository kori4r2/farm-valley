using System;
using UnityEngine;

namespace FarmValley {
    [Serializable]
    public class InventorySlot {
        [SerializeField] private Item item = null;
        public Item Item => item;
        [SerializeField] private int count = 0;
        public int Count => count;

        public bool IsEmpty => item is null || !item || count == 0;

        public void RemoveItem() {
            item = null;
            count = 0;
        }

        public void SetItem(Item newItem, int startingCount = 1) {
            if (newItem is null || !newItem || startingCount == 0) return;
            if (newItem.Stackable && startingCount <= 0) return;

            item = newItem;
            count = newItem.Stackable ? startingCount : -1;
        }

        public void AddStack(int add = 1) {
            if (IsEmpty || !item.Stackable) return;

            count += Mathf.Max(0, add);
        }

        public void RemoveStack(int remove = 1) {
            if (IsEmpty || !item.Stackable) return;

            count -= Mathf.Max(0, remove);
            if (count <= 0) RemoveItem();
        }

        private void MoveContentTo(InventorySlot other) {
            other.SetItem(item, count);
            RemoveItem();
        }

        public void SwapContents(InventorySlot other) {
            if (IsEmpty && other.IsEmpty) return;

            if (IsEmpty && !other.IsEmpty) {
                other.MoveContentTo(this);
                return;
            }

            if (!IsEmpty && other.IsEmpty) {
                MoveContentTo(other);
                return;
            }

            Item previousItem = item;
            int previousCount = count;
            other.MoveContentTo(this);
            other.SetItem(previousItem, previousCount);
        }
    }
}