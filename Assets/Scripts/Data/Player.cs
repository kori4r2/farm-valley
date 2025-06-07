using UnityEngine;

namespace FarmValley {
    [CreateAssetMenu(menuName = "FarmValley/Player")]
    public class Player : ScriptableObject {
        private PlayerController controller = null;

        public bool HasController => controller is not null && controller;

        public Vector3 GetPosition() {
            return !HasController ? Vector3.zero : controller.transform.position;
        }

        public void SetPlayer(PlayerController newPlayer) {
            controller = newPlayer;
        }

        public void UnsetPlayer(PlayerController removedPlayer) {
            if (controller == removedPlayer) controller = null;
        }

        public bool IsPlayer(GameObject obj) {
            return HasController && obj == controller.gameObject;
        }
    }
}