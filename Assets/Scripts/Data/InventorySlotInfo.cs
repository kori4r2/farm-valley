using System;

namespace FarmValley {
    [Serializable]
    public class InventorySlotInfo {
        public string itemId;
        public int index;
        public int count;

        public InventorySlotInfo(InventorySlot slot, int slotIndex) {
            itemId = slot.Item.Id;
            count = slot.Count;
            index = slotIndex;
        }
    }
}