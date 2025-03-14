using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace SmelterGame.Crafting
{
    [Serializable]
    public class CraftingRequirement
    {
        [field: OdinSerialize]
        public IProcessable ProcessableItem { get; private set; }
        [field: SerializeField, MinValue(1)]
        public int RequiredAmount { get; private set; } = 1;
    }
}
