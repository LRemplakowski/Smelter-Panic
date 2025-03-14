using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SmelterGame.Inventory
{
    [CreateAssetMenu(fileName = "New Inventory Preset", menuName = "Inventory/Default Preset")]
    public class DefaultInventoryPreset : SerializedScriptableObject, IInventoryPreset
    {
        [SerializeField]
        private List<InventoryEntry> _inventoryContents = new();

        public IReadOnlyCollection<InventoryEntry> GetPresetContents() => _inventoryContents;
    }
}