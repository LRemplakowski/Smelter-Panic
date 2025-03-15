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
        private readonly Dictionary<Guid, IBonus> _activeBonuses = new();
        [ShowInInspector, ReadOnly]
        private readonly Dictionary<BonusCategory, IBonus> _cachedHighestBonuses = new();

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
                    GetActiveBonuses().Remove(bonusDefinition.GetID());
                }
                else
                {
                    GetActiveBonuses()[bonusDefinition.GetID()] = bonusDefinition.GetBonusFactory().Create();
                }
                UpdateHighestBonusCache();
            }
        }

        public bool TryGetBonus(BonusCategory bonusCategory, out IBonus bonus)
        {
            return GetCachedHighestBonuses().TryGetValue(bonusCategory, out bonus);
        }

        private void UpdateHighestBonusCache()
        {
            GetCachedHighestBonuses().Clear();
            foreach (var bonus in GetActiveBonuses().Values)
            {
                var bonusCategory = bonus.GetBonusCategory();
                if (GetCachedHighestBonuses().TryGetValue(bonusCategory, out var current))
                {
                    var comparison = bonus.CompareTo(current);
                    if (comparison > 0)
                    {
                        GetCachedHighestBonuses()[bonusCategory] = bonus;
                    }
                }
                else
                {
                    GetCachedHighestBonuses()[bonusCategory] = bonus;
                }
            }
        }

        private Dictionary<Guid, IBonus> GetActiveBonuses() => _activeBonuses;
        private Dictionary<BonusCategory, IBonus> GetCachedHighestBonuses() => _cachedHighestBonuses;
    }
}
