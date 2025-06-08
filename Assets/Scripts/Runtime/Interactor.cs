using System.Collections.Generic;
using UnityEngine;

namespace FarmValley {
    [CreateAssetMenu(menuName = "FarmValley/Interactor")]
    public class Interactor : ScriptableObject {
        private readonly List<Interactable> interactables = new List<Interactable>();

        public void AttemptInteraction() {
            if (interactables.Count < 1) return;

            interactables[^1].Interact();
        }

        public void AddInteractable(Interactable newObj) {
            DeselectCurrentInteractable();
            RemoveInteractable(newObj);
            interactables.Add(newObj);
            SelectLastInteractable();
        }

        private void DeselectCurrentInteractable() {
            if (interactables.Count > 0) interactables[^1].Deselect();
        }

        private void SelectLastInteractable() {
            if (interactables.Count > 0) interactables[^1].Select();
        }

        public void RemoveInteractable(Interactable removedObj) {
            if (!interactables.Contains(removedObj)) return;

            removedObj.Deselect();
            interactables.Remove(removedObj);
            SelectLastInteractable();
        }
    }
}