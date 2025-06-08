using UnityEngine;

namespace FarmValley {
    [CreateAssetMenu(menuName = "FarmValley/Item")]
    public class Item : ScriptableObject {
        [SerializeField] private Sprite icon;
        public Sprite Icon => icon;
        [SerializeField] private string displayName;
        public string DisplayName => displayName;
        [SerializeField] private bool stackable;
        public bool Stackable => stackable;
        [SerializeField, TextArea] private string description;
        public string Description => description;
    }
}