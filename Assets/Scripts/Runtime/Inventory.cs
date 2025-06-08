using System.Collections.Generic;
using UnityEngine;

namespace FarmValley {
    [CreateAssetMenu(menuName = "FarmValley/Inventory")]
    public class Inventory : ScriptableObject {
        private const int inventorySize = 20;
        private InventorySlot[] slots = new InventorySlot[inventorySize];
        private Dictionary<Item, int> stackables = new Dictionary<Item, int>();
        private int firstFreeSlot = 0;
        private int freeSlots = inventorySize;

        public bool IsFull => firstFreeSlot >= inventorySize;

        private void Reset() {
            OnEnable();
        }

        private void OnEnable() {
            for (int index = 0; index < inventorySize; index++) {
                int callbackIdx = index;
                slots[index] = new InventorySlot();
                slots[index].ObserveChange(() => UpdateFirstFreeSlot(callbackIdx));
            }
            firstFreeSlot = 0;
            freeSlots = inventorySize;
            stackables.Clear();
        }

        private void UpdateFirstFreeSlot(int changeIndex) {
            if (slots[changeIndex].IsEmpty && changeIndex < firstFreeSlot) {
                firstFreeSlot = changeIndex;
                Debug.Log($"firstFreeSlot set to {firstFreeSlot}");
            } else if (changeIndex == firstFreeSlot && !slots[changeIndex].IsEmpty) {
                FindNextFreeSlot();
            }
        }

        public bool AddItem(Item item, int count = 1) {
            if (IsFull) {
                Debug.Log("Inventory is full");
                return false;
            }
            if (!item.Stackable && count > freeSlots) {
                Debug.Log("Not enough free slots");
                return false;
            }

            count = Mathf.Max(1, count);
            if (item.Stackable && stackables.TryGetValue(item, out int index)) {
                slots[index].AddStack(count);
            } else if (item.Stackable) {
                slots[firstFreeSlot].SetItem(item, count);
                stackables[item] = firstFreeSlot;
                freeSlots--;
            } else {
                for (int i = 0; i < count; i++) {
                    slots[firstFreeSlot].SetItem(item, 1);
                }
                freeSlots -= count;
            }
            return true;
        }

        private void FindNextFreeSlot() {
            while (firstFreeSlot < inventorySize && !slots[firstFreeSlot].IsEmpty) {
                firstFreeSlot++;
            }
            Debug.Log($"firstFreeSlot set to {firstFreeSlot}");
        }

        public bool RemoveItem(Item item, int count = 1) {
            int index = FindItemIndex(item);
            if (index < 0) return false;

            if (item.Stackable) {
                if (slots[index].Count < count) return false;

                slots[index].RemoveStack(count);
            } else {
                slots[index].RemoveItem();
            }
            return true;
        }

        private int FindItemIndex(Item item) {
            if (item.Stackable) return stackables.GetValueOrDefault(item, -1);

            for (int index = 0; index < inventorySize; index++) {
                if (slots[index].Item == item) return index;
            }
            return -1;
        }
    }
}