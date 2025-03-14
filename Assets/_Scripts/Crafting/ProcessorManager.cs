using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using SmelterGame.Inventory;
using UnityEngine;

namespace SmelterGame.Crafting
{
    public delegate void CraftingCompletedCallback(in Guid processorID, bool isSuccess, CraftingYield result);
    public delegate void CraftingStartedCallback(in Guid processorID, IRecipeData recipe, in Func<float> progressDelegate);

    public class ProcessorManager : SerializedMonoBehaviour
    {
        [Title("Config")]
        [SerializeField]
        private IInventory _inventory;
        [SerializeField]
        private List<IProcessorDefinition> _defaultUnlockedProcessors = new();
        [Title("Runtime")]
        [ShowInInspector, DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.ExpandedFoldout, IsReadOnly = true)]
        private readonly Dictionary<Guid, IProcessor> _unlockedProcessors = new();

        public static event CraftingStartedCallback OnCraftingStart;
        public static event CraftingCompletedCallback OnCraftingComplete;

        private void Awake()
        {
            UnlockDefaultProcessors();
        }

        private void UnlockDefaultProcessors()
        {
            _defaultUnlockedProcessors.ForEach(definition => UnlockProcessor(definition));
        }

        public bool UnlockProcessor(IProcessorDefinition processorDefinition)
        {
            if (_unlockedProcessors.ContainsKey(processorDefinition.GetID()))
            {
                Debug.LogWarning($"{nameof(ProcessorManager)} >>> Tried to unlock processor {processorDefinition} but it is already unlocked!");
                return false;
            }
            var processor = new CraftingProcessor(processorDefinition, _inventory);
            processor.OnCraftingFinished += CraftingFinished;
            _unlockedProcessors.Add(processor.GetID(), processor);
            return true;
        }

        [BoxGroup("Editor Buttons")]
        [Button, HideInEditorMode]
        public void RunRecipe(IProcessorDefinition processorDefinition, IRecipeData recipe) => RunRecipe(processorDefinition.GetID(), recipe);

        [BoxGroup("Editor Buttons")]
        [Button, HideInEditorMode]
        public void RunRecipe(Guid processorID, IRecipeData recipe)
        {
            if (_unlockedProcessors.TryGetValue(processorID, out var processor))
            {
                processor.CraftRecipe(recipe, out var progressDelegate);
                CraftingStarted(in processorID, recipe, in progressDelegate);
            }
        }

        private void CraftingFinished(in Guid processorID, bool isSuccess, CraftingYield result)
        {
            OnCraftingComplete?.Invoke(in processorID, isSuccess, result);
        }

        private void CraftingStarted(in Guid processorID, IRecipeData recipe, in Func<float> progressDelegate)
        {
            OnCraftingStart?.Invoke(in processorID, recipe, in progressDelegate);
        }
    }
}
