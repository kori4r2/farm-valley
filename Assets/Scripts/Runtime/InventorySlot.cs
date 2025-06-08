using System;
using UnityEngine;
using UnityEngine.Events;

namespace FarmValley {
    [Serializable]
    public class InventorySlot {
        [SerializeField] private Item item = null;
        public Item Item => item;
        [SerializeField] private int count = 0;
        public int Count => count;

        public bool IsEmpty => item is null || !item || count == 0;
        private UnityEvent onChange = new UnityEvent();

        public void ObserveChange(UnityAction callback) {
            onChange.AddListener(callback);
        }

        public void StopObserving(UnityAction callback) {
            onChange.RemoveListener(callback);
        }

        public void RemoveItem() {
            item = null;
            count = 0;
            onChange.Invoke();
        }

        public void SetItem(Item newItem, int startingCount = 1) {
            if (newItem is null || !newItem || startingCount == 0) return;
            if (newItem.Stackable && startingCount <= 0) return;

            item = newItem;
            count = newItem.Stackable ? startingCount : -1;
            onChange.Invoke();
        }

        public void AddStack(int add = 1) {
            if (IsEmpty || !item.Stackable) return;

            count += Mathf.Max(0, add);
            onChange.Invoke();
        }

        public void RemoveStack(int remove = 1) {
            if (IsEmpty || !item.Stackable) return;

            count -= Mathf.Max(0, remove);
            if (count <= 0)
                RemoveItem();
            else
                onChange.Invoke();
        }

        public void SwapContents(InventorySlot other) {
            if (IsEmpty && other.IsEmpty) return;

            Item previousItem = item;
            int previousCount = count;
            SetItem(other.item, other.count);
            other.SetItem(previousItem, previousCount);
        }
    }
}