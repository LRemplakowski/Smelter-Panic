using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using SmelterGame.Inventory;
using UnityEngine;

namespace SmelterGame.Bonuses
{
    public class BonusManager : MonoBehaviour, IBonusProvider
    {
        [ShowInInspector, ReadOnly]
        private readonly Dictionary<Guid, IBonus> _existingBonuses = new();
        [ShowInInspector, ReadOnly]
        private readonly Dictionary<BonusCategory, IBonus> _cachedActiveBonuses = new();

        public static event Action<IReadOnlyCollection<IBonus>> OnActiveBonusesUpdated;

        private void Awake()
        {
            InventoryManager.OnInventoryUpdated += OnInventoryUpdated;
        }

        private void OnInventoryUpdated(IItem item, int amount)
        {
            if (item is IBonusDefinition bonusDefinition)
            {
                if (amount <= 0)
                {
                    GetExistingBonuses().Remove(bonusDefinition.GetID());
                }
                else
                {
                    GetExistingBonuses()[bonusDefinition.GetID()] = bonusDefinition.GetBonusFactory().Create();
                }
                UpdateHighestBonusCache();
            }
        }

        public bool TryGetBonus(BonusCategory bonusCategory, out IBonus bonus)
        {
            return GetCachedActiveBonuses().TryGetValue(bonusCategory, out bonus);
        }

        private void UpdateHighestBonusCache()
        {
            GetCachedActiveBonuses().Clear();
            foreach (var bonus in GetExistingBonuses().Values)
            {
                var bonusCategory = bonus.GetBonusCategory();
                if (GetCachedActiveBonuses().TryGetValue(bonusCategory, out var current))
                {
                    var comparison = bonus.CompareTo(current);
                    if (comparison > 0)
                    {
                        GetCachedActiveBonuses()[bonusCategory] = bonus;
                    }
                }
                else
                {
                    GetCachedActiveBonuses()[bonusCategory] = bonus;
                }
            }
            OnActiveBonusesUpdated?.Invoke(GetCachedActiveBonuses().Values);
        }

        private Dictionary<Guid, IBonus> GetExistingBonuses() => _existingBonuses;
        private Dictionary<BonusCategory, IBonus> GetCachedActiveBonuses() => _cachedActiveBonuses;
    }
}
