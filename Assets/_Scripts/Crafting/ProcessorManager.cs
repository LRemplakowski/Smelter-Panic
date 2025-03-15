using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using SmelterGame.Bonuses;
using SmelterGame.Inventory;
using UnityEngine;

namespace SmelterGame.Crafting
{
    public delegate void CraftingCompletedCallback(in Guid processorID, bool isSuccess, CraftingYield result);
    public delegate void CraftingStartedCallback(in Guid processorID, IRecipeData recipe, in Func<float> progressDelegate);
    public delegate void ProcessorUnlockedDelegate(IProcessorDefinition processorDefinition);
    public delegate bool RecipeValidationDelegate(in Guid processorID, IRecipeData recipeData);

    public class ProcessorManager : SerializedMonoBehaviour
    {
        [Title("Config")]
        [SerializeField]
        private IInventory _inventory;
        [SerializeField]
        private IBonusProvider _bonusProvider;
        [SerializeField]
        private List<IProcessorDefinition> _defaultUnlockedProcessors = new();
        [Title("Runtime")]
        [ShowInInspector, DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.ExpandedFoldout, IsReadOnly = true)]
        private readonly Dictionary<Guid, IProcessor> _unlockedProcessors = new();
        [ShowInInspector, ReadOnly]
        private readonly HashSet<Guid> _activeProcessors = new();

        public static event CraftingStartedCallback OnCraftingStart;
        public static event CraftingCompletedCallback OnCraftingComplete;
        public static event ProcessorUnlockedDelegate OnProcessorUnlocked;

        private IProcessorFactory _processorFactory;

        private void Awake()
        {
            EnsureProcessorFactory();
        }

        private void Start()
        {
            UnlockDefaultProcessors();
        }

        public bool UnlockProcessor(IProcessorDefinition processorDefinition)
        {
            if (GetUnlockedProcessors().ContainsKey(processorDefinition.GetID()))
            {
                Debug.LogWarning($"{nameof(ProcessorManager)} >>> Tried to unlock processor {processorDefinition} but it is already unlocked!");
                return false;
            }
            var processor = GetProcessorFactory().Create(processorDefinition);
            processor.OnCraftingFinished += CraftingFinished;
            GetUnlockedProcessors().Add(processor.GetID(), processor);
            OnProcessorUnlocked?.Invoke(processorDefinition);
            return true;
        }

        public bool CanCraftRecipe(in Guid processorID, IRecipeData recipe)
        {
            return GetUnlockedProcessors().TryGetValue(processorID, out var processor) && processor.CanCraftRecipe(recipe);
        }

        [BoxGroup("Editor Buttons")]
        [Button, HideInEditorMode]
        public void RunRecipe(IProcessorDefinition processorDefinition, IRecipeData recipe) => RunRecipe(processorDefinition.GetID(), recipe);

        [BoxGroup("Editor Buttons")]
        [Button, HideInEditorMode]
        public void RunRecipe(Guid processorID, IRecipeData recipe)
        {
            if (!IsActiveProcessor(processorID) && GetUnlockedProcessors().TryGetValue(processorID, out var processor))
            {
                processor.CraftRecipe(recipe, out var progressDelegate);
                CraftingStarted(in processorID, recipe, in progressDelegate);
            }
        }

        private void UnlockDefaultProcessors()
        {
            GetDefaultUnlockedProcessors().ForEach(definition => UnlockProcessor(definition));
        }

        private void EnsureProcessorFactory()
        {
            _processorFactory ??= new DefaultProcessorFactory(GetInventory(), GetBonusProvider());
        }

        private void CraftingFinished(in Guid processorID, bool isSuccess, CraftingYield result)
        {
            _activeProcessors.Remove(processorID);
            OnCraftingComplete?.Invoke(in processorID, isSuccess, result);
        }

        private void CraftingStarted(in Guid processorID, IRecipeData recipe, in Func<float> progressDelegate)
        {
            _activeProcessors.Add(processorID);
            OnCraftingStart?.Invoke(in processorID, recipe, in progressDelegate);
        }

        private IInventory GetInventory() => _inventory;
        private IBonusProvider GetBonusProvider() => _bonusProvider;
        private IProcessorFactory GetProcessorFactory() => _processorFactory;
        private ICollection<IProcessorDefinition> GetDefaultUnlockedProcessors() => _defaultUnlockedProcessors;
        private Dictionary<Guid, IProcessor> GetUnlockedProcessors() => _unlockedProcessors;
        private bool IsActiveProcessor(Guid processorID) => _activeProcessors.Contains(processorID);


        private interface IProcessorFactory
        {
            IProcessor Create(IProcessorDefinition definition);
        }

        private class DefaultProcessorFactory : IProcessorFactory
        {
            private readonly IInventory _processorInventory;
            private readonly IBonusProvider _bonusProvider;

            public DefaultProcessorFactory(IInventory processorInventory, IBonusProvider bonusProvider)
            {
                _processorInventory = processorInventory;
                _bonusProvider = bonusProvider;
            }

            public IProcessor Create(IProcessorDefinition processorDefinition)
            {
                return new CraftingProcessor(processorDefinition, _processorInventory, _bonusProvider);
            }
        }
    }
}
