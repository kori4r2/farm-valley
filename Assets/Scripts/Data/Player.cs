using UnityEngine;

namespace FarmValley {
    [CreateAssetMenu(menuName = "FarmValley/Player")]
    public class Player : ScriptableObject {
        private PlayerController controller = null;

        public void SetPlayer(PlayerController newPlayer) {
            this.controller = newPlayer;
        }

        public void UnsetPlayer(PlayerController removedPlayer) {
            if (this.controller == removedPlayer) this.controller = null;
        }

        public bool IsPlayer(GameObject obj) {
            return controller is not null && controller && obj == controller.gameObject;
        }
    }
}