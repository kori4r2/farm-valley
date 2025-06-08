using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FarmValley {
    [CreateAssetMenu(menuName = "FarmValley/SaveLoader")]
    public class SaveLoader : ScriptableObject {
        [SerializeField] private Inventory inventory;
        [SerializeField] private List<Item> itemList = new();
        private Dictionary<string, Item> itemsById;
        private const string saveFileName = "SaveFile";

        private void Reset() {
            OnEnable();
        }

        private void OnEnable() {
            UpdateItemDict();
        }

        [ContextMenu("Update Dictionary")]
        public void UpdateItemDict() {
            if (itemsById != null)
                itemsById.Clear();
            else
                itemsById = new Dictionary<string, Item>();
            foreach (Item item in itemList) {
                itemsById.Add(item.Id, item);
            }
        }

        public void Save() {
            InventoryInfo info = new InventoryInfo(inventory);
            string jsonString = JsonUtility.ToJson(info);
            string filePath = Path.Combine(Application.persistentDataPath, saveFileName);
            using StreamWriter streamWriter = File.CreateText(filePath);
            streamWriter.Write(jsonString);
            streamWriter.Close();
        }

        public void Load() {
            inventory.Reset();
            InventoryInfo info = LoadFromFile();

            info?.LoadInfo(inventory, itemsById);
        }

        private static InventoryInfo LoadFromFile() {
            string filePath = Path.Combine(Application.persistentDataPath, saveFileName);
            if (!File.Exists(filePath)) return null;

            using StreamReader streamReader = File.OpenText(filePath);
            string jsonString = streamReader.ReadToEnd();
            if (jsonString.Length <= 0) {
                streamReader.Close();
                return null;
            }

            InventoryInfo info = JsonUtility.FromJson<InventoryInfo>(jsonString);
            streamReader.Close();
            return info;
        }
    }
}