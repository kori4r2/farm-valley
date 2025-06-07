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
            RemoveInteractable(newObj);
            interactables.Add(newObj);
        }

        public void RemoveInteractable(Interactable removedObj) {
            if (interactables.Contains(removedObj)) interactables.Remove(removedObj);
        }
    }
}