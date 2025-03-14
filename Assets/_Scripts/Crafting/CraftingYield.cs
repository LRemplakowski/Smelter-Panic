using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace SmelterGame.Crafting
{
    [Serializable]
    public class CraftingYield
    {
        [field: OdinSerialize]
        public ICraftable YieldItem { get; private set; }
        [field: SerializeField, MinValue(1)]
        public int YieldAmount { get; private set; } = 1;
    }
}