using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FarmValley {
    public class InventorySlotView : MonoBehaviour {
        [SerializeField] private Image itemIcon;
        [SerializeField] private Image selectionBorder;
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI stackCounter;
        private InventorySlot slot;
        [Header("ItemDrag")]
        [SerializeField] private InventoryDragItem dragItem;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Color defaultTint;
        [SerializeField] private Color selectedTint;
        [SerializeField] private Color targetTint;

        public void Init(InventorySlot inventorySlot) {
            slot = inventorySlot;
            selectionBorder.gameObject.SetActive(false);
            LoadSlotInfo();
            slot.ObserveChange(LoadSlotInfo);
        }

        private void LoadSlotInfo() {
            itemIcon.gameObject.SetActive(!slot.IsEmpty);
            itemName.gameObject.SetActive(!slot.IsEmpty);
            stackCounter.gameObject.SetActive(!slot.IsEmpty && slot.Item.Stackable);
            backgroundImage.color = defaultTint;
            if (slot.IsEmpty) return;

            if (slot.Item.Stackable) {
                stackCounter.text = $"x{slot.Count}";
            }
            itemName.text = slot.Item.DisplayName;
            itemIcon.sprite = slot.Item.Icon;
        }

        private void OnDestroy() {
            slot?.StopObserving(LoadSlotInfo);
        }

        public void OnPointerEnter() {
            selectionBorder.gameObject.SetActive(true);
            if (!dragItem.IsDraggingSomething) return;

            dragItem.SetTarget(slot);
            backgroundImage.color = targetTint;
        }

        public void OnPointerExit() {
            selectionBorder.gameObject.SetActive(false);
            if (!dragItem.IsDraggingSomething) return;

            dragItem.RemoveIfTarget(slot);
            backgroundImage.color = dragItem.IsBeingDragged(slot) ? selectedTint : defaultTint;
        }

        public void OnPointerDown() {
            if (slot == null || slot.IsEmpty) return;

            dragItem.BeginDrag(slot);
            backgroundImage.color = selectedTint;
        }

        public void OnPointerUp() {
            dragItem.FinishDrag();
            backgroundImage.color = defaultTint;
        }
    }
}