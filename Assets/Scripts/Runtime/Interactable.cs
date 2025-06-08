using UnityEngine;

namespace FarmValley {
    public abstract class Interactable : MonoBehaviour {
        [SerializeField] protected Interactor interactor;
        [SerializeField] protected Player player;
        [SerializeField] protected Collider2D trigger;

        protected void OnDisable() {
            interactor.RemoveInteractable(this);
        }

        protected virtual void OnTriggerEnter2D(Collider2D other) {
            if (!player.IsPlayer(other.gameObject)) return;

            interactor.AddInteractable(this);
        }

        protected virtual void OnTriggerExit2D(Collider2D other) {
            if (!player.IsPlayer(other.gameObject)) return;

            interactor.RemoveInteractable(this);
        }

        public abstract void Interact();

        public abstract void Select();

        public abstract void Deselect();
    }
}