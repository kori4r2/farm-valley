using Toblerone.Toolbox;
using UnityEngine;

namespace FarmValley {
    public class InventoryDescriptionView : MonoBehaviour {
        [SerializeField] private VariableObserver<string> descriptionObserver;

        private void Awake() {
            gameObject.SetActive(false);
            descriptionObserver.StartWatching();
        }

        private void OnDestroy() {
            descriptionObserver.StopWatching();
        }

        public void ShowIfValidString(string description) {
            gameObject.SetActive(!string.IsNullOrEmpty(description));
        }
    }
}