using System.Collections.Generic;
using Toblerone.Toolbox;
using UnityEngine;

namespace FarmValley {
    public class InventoryView : MonoBehaviour {
        [SerializeField] private Inventory inventory;
        [SerializeField] private SaveLoader saveLoader;
        [SerializeField] private InventorySlotView slotViewPrefab;
        [SerializeField] private Transform slotsParent;
        [SerializeField] private EventListener activationListener;
        [SerializeField] private List<PlayerInput> deactivationInputs;

        private void Awake() {
            activationListener.StartListeningEvent();
        }

        private void Start() {
            saveLoader.Load();
            foreach (Transform child in slotsParent) {
                Destroy(child.gameObject);
            }
            for (int index = 0; index < Inventory.InventorySize; index++) {
                CreateNewSlot(index);
            }
            Hide();
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

        private void OnDestroy() {
            activationListener.StopListeningEvent();
        }

        public void Show() {
            gameObject.SetActive(true);
        }

        public void Hide() {
            gameObject.SetActive(false);
        }

        private void CreateNewSlot(int index) {
            InventorySlotView newView = Instantiate(slotViewPrefab, slotsParent);
            newView.Init(inventory.GetSlot(index));
        }
    }
}