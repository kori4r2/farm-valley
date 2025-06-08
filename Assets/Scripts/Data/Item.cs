using UnityEngine;

namespace FarmValley {
    [CreateAssetMenu(menuName = "FarmValley/Item")]
    public class Item : ScriptableObject {
        [SerializeField] private Sprite icon;
        [SerializeField] private string displayName;
        [SerializeField] private bool stackable;
        [SerializeField] [TextArea] private string description;
        public Sprite Icon => icon;
        public string DisplayName => displayName;
        public bool Stackable => stackable;
        public string Description => description;
    }
}