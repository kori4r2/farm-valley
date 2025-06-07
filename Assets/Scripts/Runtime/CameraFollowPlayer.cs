using UnityEngine;

namespace FarmValley {
    [RequireComponent(typeof(Camera))]
    public class CameraFollowPlayer : MonoBehaviour {
        [SerializeField] private Player player;
        private Camera followCamera;
        private Vector3 targetPosition;

        private void Awake() {
            followCamera = GetComponent<Camera>();
        }

        private void LateUpdate() {
            if (!player.HasController) return;

            targetPosition = player.GetPosition();
            followCamera.transform.position =
                new Vector3(targetPosition.x, targetPosition.y, followCamera.transform.position.z);
        }
    }
}