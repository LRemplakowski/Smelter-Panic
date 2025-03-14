using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace SmelterGame.Crafting
{
    [CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Crafting/Crafting Recipe")]
    public class CraftingRecipe : SerializedScriptableObject, IRecipeData
    {
        [SerializeField]
        private Guid _recipeID = Guid.NewGuid();
        [Title("Core Config")]
        [SerializeField, MinValue(0.1f)]
        private float _processingTime = 1f;
        [SerializeField, PropertyRange(0.001d, 1d)]
        private double _successChance = 1d;
        [Title("References")]
        [OdinSerialize, InlineProperty(), LabelWidth(100), HideReferenceObjectPicker]
        private CraftingYield _craftingResult = new();
        [OdinSerialize, HideReferenceObjectPicker]
        private List<CraftingRequirement> _requiredResources = new();

        public Guid GetID() => _recipeID;
        public CraftingYield GetCraftingResult() => _craftingResult;
        public float GetProcessingTime() => _processingTime;
        public IReadOnlyCollection<CraftingRequirement> GetRequiredResources() => _requiredResources;
        public double GetSuccessChance() => _successChance;
    }
}
