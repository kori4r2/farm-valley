using Toblerone.Toolbox;
using UnityEngine;

namespace FarmValley {
    public class GameTimer : MonoBehaviour {
        [SerializeField] private FloatVariable time;
        [SerializeField] private BoolVariable isPaused;
        private VariableObserver<bool> pauseObserver;

        private void Awake() {
            time.Value = 0;
            pauseObserver = new VariableObserver<bool>(isPaused, OnPauseChanged);
            pauseObserver.StartWatching();
        }

        private void OnPauseChanged(bool paused) {
            enabled = !paused;
        }

        private void OnDestroy() {
            pauseObserver.StopWatching();
        }

        private void Update() {
            time.Value += Time.deltaTime;
        }
    }
}