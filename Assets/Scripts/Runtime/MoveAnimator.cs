using System;
using UnityEngine;

namespace FarmValley {
    [Serializable]
    public class MoveAnimator {
        [SerializeField] private Animator animator;
        [SerializeField] private string runningAnimatorKey;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private bool defaultLookRight;
        private int runningAnimatorKeyId;

        public void Init() {
            runningAnimatorKeyId = Animator.StringToHash(runningAnimatorKey);
        }

        public void Update(Vector2 velocity) {
            if (Mathf.Abs(velocity.x) > Mathf.Epsilon) AdjustHorizontalFlip(velocity);
            animator.SetBool(runningAnimatorKeyId, velocity.sqrMagnitude > Mathf.Epsilon * Mathf.Epsilon);
        }

        private void AdjustHorizontalFlip(Vector2 velocity) {
            spriteRenderer.flipX = (!(velocity.x > Mathf.Epsilon) || !defaultLookRight)
                                   && (!(velocity.x < -Mathf.Epsilon) || defaultLookRight);
        }
    }
}