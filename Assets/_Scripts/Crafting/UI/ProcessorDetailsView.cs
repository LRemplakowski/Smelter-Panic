using System;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace SmelterGame.Crafting.UI
{
    public class ProcessorDetailsView : MonoBehaviour
    {
        [SerializeField]
        private CraftingRecipeSlotView _recipeSlotPrefab;
        [SerializeField]
        private Transform _recipeSlotParent;
        [SerializeField]
        private Image _viewBackground;

        private IProcessorDefinition _cachedProcessorDefinition;
        private Action<Guid, IRecipeData> _recipeConfirmedDelegate;
        private RecipeValidationDelegate _processorValidationDelegate;
        private IRecipeSlotFactory _recipeSlotFactory;

        private void Awake()
        {
            EnsureRecipeSlotFactory();
        }

        public void SetEnabled(bool enabled)
        {
            gameObject.SetActive(enabled);
        }

        // Could use a builder
        public void Initialize(IProcessorDefinition processorDefinition,
                               Action<Guid, IRecipeData> recipeConfirmedDelegate,
                               RecipeValidationDelegate processorValidationDelegate)
        {
            _cachedProcessorDefinition = processorDefinition;
            _recipeConfirmedDelegate = recipeConfirmedDelegate;
            _processorValidationDelegate = processorValidationDelegate;
            _viewBackground.sprite = processorDefinition.GetIcon();
            CreateRecipeViews();
        }

        private void CreateRecipeViews()
        {
            var recipes = GetCachedProcessorDefinition().GetAcceptedRecipes();
            EnsureRecipeSlotFactory();
            recipes.ForEach(recipe => GetRecipeSlotFactory().Create(GetCachedProcessorDefinition().GetID(), recipe, GetProcessorValidationDelegate()));
        }

        private void OnRecipeConfirmed(IRecipeData recipeData)
        {
            GetRecipeConfirmedDelegate()?.Invoke(GetCachedProcessorDefinition().GetID(), recipeData);
        }

        private void EnsureRecipeSlotFactory()
        {
            _recipeSlotFactory ??= new DefaultRecipeSlotFactory(GetRecipeSlotPrefab(), GetRecipeSlotParent(), OnRecipeConfirmed);
        }

        private CraftingRecipeSlotView GetRecipeSlotPrefab() => _recipeSlotPrefab;
        private Transform GetRecipeSlotParent() => _recipeSlotParent;
        private IProcessorDefinition GetCachedProcessorDefinition() => _cachedProcessorDefinition;
        private Action<Guid, IRecipeData> GetRecipeConfirmedDelegate() => _recipeConfirmedDelegate;
        private RecipeValidationDelegate GetProcessorValidationDelegate() => _processorValidationDelegate;
        private IRecipeSlotFactory GetRecipeSlotFactory() => _recipeSlotFactory;
        
        private interface IRecipeSlotFactory
        {
            CraftingRecipeSlotView Create(in Guid proecssorID, IRecipeData recipeData, RecipeValidationDelegate processorValidationDelegate);
        }

        private class DefaultRecipeSlotFactory : IRecipeSlotFactory
        {
            private readonly CraftingRecipeSlotView _slotPrefab;
            private readonly Transform _slotParent;
            private readonly Action<IRecipeData> _recipeConfirmedDelegate;

            public DefaultRecipeSlotFactory(CraftingRecipeSlotView slotPrefab, Transform slotParent, Action<IRecipeData> recipeConfirmedDelegate)
            {
                _slotPrefab = slotPrefab;
                _slotParent = slotParent;
                _recipeConfirmedDelegate = recipeConfirmedDelegate;
            }

            public CraftingRecipeSlotView Create(in Guid proecssorID, IRecipeData recipeData, RecipeValidationDelegate processorValidationDelegate)
            {
                var slotView = Instantiate(_slotPrefab, _slotParent);
                slotView.Initialize(proecssorID, recipeData, _recipeConfirmedDelegate, processorValidationDelegate);
                return slotView;
            }
        }
    }
}
