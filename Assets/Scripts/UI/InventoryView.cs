using System.Collections.Generic;
using Toblerone.Toolbox;
using UnityEngine;

namespace FarmValley {
    public class InventoryView : MonoBehaviour {
        [SerializeField] private Inventory inventory;
        [SerializeField] private InventorySlotView slotViewPrefab;
        [SerializeField] private Transform slotsParent;
        [SerializeField] private EventListener activationListener;
        [SerializeField] private List<PlayerInput> deactivationInputs;

        public void Show() {
            gameObject.SetActive(true);
        }

        public void Hide() {
            gameObject.SetActive(false);
        }

        private void Awake() {
            activationListener.StartListeningEvent();
        }

        private void OnDestroy() {
            activationListener.StopListeningEvent();
        }

        private void Start() {
            foreach (Transform child in slotsParent) {
                Destroy(child.gameObject);
            }
            for (int index = 0; index < Inventory.InventorySize; index++) {
                CreateNewSlot(index);
            }
            Hide();
        }

        private void CreateNewSlot(int index) {
            InventorySlotView newView = Instantiate(slotViewPrefab, slotsParent);
            newView.Init(inventory.GetSlot(index));
        }

        private void OnEnable() {
            foreach (PlayerInput deactivationInput in deactivationInputs) {
                deactivationInput.SetupCallbacks();
            }
        }

        private void OnDisable() {
            foreach (PlayerInput deactivationInput in deactivationInputs) {
                deactivationInput.RemoveCallbacks();
            }
        }
    }
}