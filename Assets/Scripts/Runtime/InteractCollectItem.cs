using Toblerone.Toolbox;
using UnityEngine;

namespace FarmValley {
    public class InteractCollectItem : Interactable {
        [SerializeField] private GameObject selectionIndicator;
        [SerializeField] private Inventory inventory;
        [SerializeField] private Item item;
        [SerializeField] [Range(1, 999)] private int count = 1;
        [SerializeField] [Tooltip("Seconds")] [Range(0, 1440)]
        private float cooldown;
        [SerializeField] private FloatVariable gameTime;
        private VariableObserver<float> timerObserver;
        private float activationTime;

        private void Awake() {
            timerObserver = new VariableObserver<float>(gameTime, OnTimerChanged);
        }

        private void OnTimerChanged(float time) {
            if (time < activationTime) return;

            timerObserver.StopWatching();
            trigger.enabled = true;
        }

        public override void Interact() {
            if (!inventory.AddItem(item, count) || cooldown == 0) return;

            interactor.RemoveInteractable(this);
            trigger.enabled = false;
            activationTime = gameTime.Value + cooldown;
            timerObserver.StartWatching();
        }

        public override void Select() {
            selectionIndicator.SetActive(true);
        }

        public override void Deselect() {
            selectionIndicator.SetActive(false);
        }
    }
}