using System.Collections.Generic;
using UnityEngine;

namespace FarmValley {
    [CreateAssetMenu(menuName = "FarmValley/Inventory")]
    public class Inventory : ScriptableObject {
        private const int inventorySize = 20;
        private InventorySlot[] slots = new InventorySlot[inventorySize];
        private Dictionary<Item, int> indexSearch = new Dictionary<Item, int>();
        private int firstFreeSlot = 0;

        public bool IsFull => firstFreeSlot >= inventorySize;

        public bool AddItem(Item item, int count = 1) {
            if (IsFull) return false;

            count = Mathf.Max(1, count);
            if (indexSearch.TryGetValue(item, out int index)) {
                if (!item.Stackable) return false;

                slots[index].AddStack(count);
            } else {
                slots[firstFreeSlot].SetItem(item, count);
                indexSearch[item] = firstFreeSlot;
                FindNextFreeSlot();
            }
            return true;
        }

        private void FindNextFreeSlot() {
            while (firstFreeSlot < inventorySize && !slots[firstFreeSlot].IsEmpty) {
                firstFreeSlot++;
            }
        }

        public bool RemoveItem(Item item, int count = 1) {
            if (!indexSearch.TryGetValue(item, out int index)) return false;

            if (item.Stackable) {
                if (slots[index].Count < count) return false;

                slots[index].RemoveStack(count);
            } else {
                slots[index].RemoveItem();
            }
            if (slots[index].IsEmpty && index < firstFreeSlot) firstFreeSlot = index;
            return true;
        }
    }
}