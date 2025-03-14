using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SmelterGame.Crafting
{
    [CreateAssetMenu(fileName = "New Processor Definition", menuName = "Crafting/Processor Definition")]
    public class ProcessorDefinitionData : SerializedScriptableObject, IProcessorDefinition
    {
        [SerializeField]
        private Guid _guid = Guid.NewGuid();

        [SerializeField]
        private List<IRecipeData> _acceptedRecipes = new();

        public Guid GetID() => _guid;
        public IReadOnlyCollection<IRecipeData> GetAcceptedRecipes() => _acceptedRecipes;
    }
}
