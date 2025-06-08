using System.Collections.Generic;
using UnityEngine;

namespace FarmValley {
    [CreateAssetMenu(menuName = "FarmValley/Inventory")]
    public class Inventory : ScriptableObject {
        public const int InventorySize = 20;
        private readonly InventorySlot[] slots = new InventorySlot[InventorySize];
        private readonly Dictionary<Item, int> stackables = new();
        private int firstFreeSlot;
        private int freeSlots = InventorySize;

        private bool IsFull => firstFreeSlot >= InventorySize;

        private void Reset() {
            OnEnable();
        }

        private void OnEnable() {
            for (int index = 0; index < InventorySize; index++) {
                int callbackIdx = index;
                slots[index] = new InventorySlot();
                slots[index].ObserveChange(() => UpdateFirstFreeSlot(callbackIdx));
            }
            firstFreeSlot = 0;
            freeSlots = InventorySize;
            stackables.Clear();
        }

        public InventorySlot GetSlot(int index) {
            index = Mathf.Clamp(index, 0, InventorySize - 1);
            return slots[index];
        }

        private void UpdateFirstFreeSlot(int changeIndex) {
            if (slots[changeIndex].IsEmpty && changeIndex < firstFreeSlot)
                firstFreeSlot = changeIndex;
            else if (changeIndex == firstFreeSlot && !slots[changeIndex].IsEmpty) FindNextFreeSlot();
        }

        public bool AddItem(Item item, int count = 1) {
            if (IsFull && (!item.Stackable || !stackables.ContainsKey(item))) return false;

            if (!item.Stackable && count > freeSlots) return false;

            count = Mathf.Max(1, count);
            if (item.Stackable && stackables.TryGetValue(item, out int index))
                slots[index].AddStack(count);
            else if (item.Stackable) {
                stackables[item] = firstFreeSlot;
                slots[firstFreeSlot].SetItem(item, count);
                freeSlots--;
            } else {
                for (int i = 0; i < count; i++) {
                    slots[firstFreeSlot].SetItem(item);
                }
                freeSlots -= count;
            }
            return true;
        }

        private void FindNextFreeSlot() {
            while (firstFreeSlot < InventorySize && !slots[firstFreeSlot].IsEmpty) {
                firstFreeSlot++;
            }
        }

        public bool RemoveItem(Item item, int count = 1) {
            int index = FindItemIndex(item);
            if (index < 0) return false;

            if (item.Stackable) {
                if (slots[index].Count < count) return false;

                slots[index].RemoveStack(count);
            } else
                slots[index].RemoveItem();
            return true;
        }

        private int FindItemIndex(Item item) {
            if (item.Stackable) return stackables.GetValueOrDefault(item, -1);

            for (int index = 0; index < InventorySize; index++) {
                if (slots[index].Item == item) return index;
            }

            return -1;
        }
    }
}