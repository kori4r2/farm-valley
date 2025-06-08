using System;
using System.Collections.Generic;
using UnityEngine;

namespace FarmValley {
    [Serializable]
    public class InventoryInfo {
        [SerializeField] private List<InventorySlotInfo> slotInfos;

        public InventoryInfo(Inventory inventory) {
            slotInfos = new List<InventorySlotInfo>();
            for (int index = 0; index < Inventory.InventorySize; index++) {
                InventorySlot slot = inventory.GetSlot(index);
                if (slot.IsEmpty) continue;

                slotInfos.Add(new InventorySlotInfo(slot, index));
            }
        }

        public void LoadInfo(Inventory inventory, Dictionary<string, Item> itemsById) {
            inventory.Reset();
            foreach (InventorySlotInfo info in slotInfos) {
                inventory.GetSlot(info.index).SetItem(itemsById[info.itemId], info.count);
            }
            inventory.RecheckStackableItems();
        }
    }
}