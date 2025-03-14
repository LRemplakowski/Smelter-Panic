using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace SmelterGame.Inventory
{
    [CreateAssetMenu(fileName = "New Randomized Inventory Preset", menuName = "Inventory/Randomized Preset")]
    public class RandomizedInventoryPreset : SerializedScriptableObject, IInventoryPreset
    {
        [OdinSerialize, HideReferenceObjectPicker]
        private List<RandomizedInventoryEntry> _presetContents = new();

        public IReadOnlyCollection<InventoryEntry> GetPresetContents()
        {
            List<InventoryEntry> inventoryContents = new();
            _presetContents.Where(entry => entry.ShouldSpawn()).ForEach(presetItem => inventoryContents.Add(presetItem.EvaluateContents()));
            return inventoryContents;
        }

        [Serializable]
        private class RandomizedInventoryEntry
        {
            [SerializeField]
            private IItem _item;
            [SerializeField, PropertyRange(0d, 1d)]
            private float _spawnChance = 1f;
            [SerializeField, MinMaxSlider(minValue: 0, maxValue: 100, showFields: true)]
            private Vector2Int _amountRange = new(1, 10);

            public bool ShouldSpawn() => UnityEngine.Random.Range(0f, 1f) <= _spawnChance;

            public InventoryEntry EvaluateContents()
            {
                return new() 
                { 
                    Item = this._item, 
                    Amount = UnityEngine.Random.Range(_amountRange.x, _amountRange.y + 1) 
                };
            }
        }
    }
}
