using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace SmelterGame.Inventory
{
    public delegate void InventoryUpdatedDelegate(IItem item, int amount);

    public class InventoryManager : SerializedMonoBehaviour, IInventory
    {
        [Title("Config")]
        [SerializeField]
        private IInventoryPreset _inventoryContentsPreset;
        [Title("Runtime")]
        [ShowInInspector, DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)]
        private readonly Dictionary<Guid, InventoryEntry> _inventoryContents = new();

        public static event InventoryUpdatedDelegate OnInventoryUpdated;

        private void Awake()
        {
            _inventoryContentsPreset?.GetPresetContents().ForEach(entry => AddItem(entry.Item, entry.Amount));
        }

        private void Start()
        {
            _inventoryContents.Values.ForEach(inventoryEntry => OnInventoryUpdated?.Invoke(inventoryEntry.Item, inventoryEntry.Amount));
        }

        public void AddItem(IItem item, int amount = 1)
        {
            var itemID = item.GetID();
            if (amount <= 0)
            {
                Debug.LogError($"{nameof(InventoryManager)} >>> Cannot add 0 or less to inventory! Amount given: {amount}");
                return;
            }
            if (_inventoryContents.TryGetValue(itemID, out var existingEntry))
            {
                existingEntry.Amount += amount;
                _inventoryContents[itemID] = existingEntry;
                OnInventoryUpdated?.Invoke(existingEntry.Item, existingEntry.Amount);
            }
            else
            {
                _inventoryContents[itemID] = new()
                {
                    Item = item,
                    Amount = amount
                };
                OnInventoryUpdated?.Invoke(item, amount);
            }
        }

        public bool ContainsItem(IItem item, int count = 1)
        {
            if (count == 1)
            {
                return _inventoryContents.ContainsKey(item.GetID());
            }
            else
            {
                return _inventoryContents.TryGetValue(item.GetID(), out var entry) && entry.Amount >= count;
            }
        }

        public bool TryRemoveItem(IItem item, int amount = 1)
        {
            var itemID = item.GetID();
            if (_inventoryContents.TryGetValue(itemID, out var entry) && entry.Amount >= amount)
            {
                entry.Amount -= amount;
                if (entry.Amount <= 0)
                {
                    _inventoryContents.Remove(itemID);
                }
                else
                {
                    _inventoryContents[itemID] = entry;
                }
                OnInventoryUpdated?.Invoke(entry.Item, entry.Amount);
                return true;
            }
            return false;
        }
    }
}
