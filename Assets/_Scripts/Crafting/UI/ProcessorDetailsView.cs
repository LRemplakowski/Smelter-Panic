using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using SmelterGame.Bonuses;
using SmelterGame.Bonuses.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SmelterGame.Crafting.UI
{
    public class ProcessorDetailsView : MonoBehaviour
    {
        [SerializeField]
        private Image _viewBackground;
        [Title("Crafting Recipes")]
        [SerializeField]
        private CraftingRecipeSlotView _recipeSlotPrefab;
        [SerializeField]
        private Transform _recipeSlotParent;
        [Title("Active Bonuses")]
        [SerializeField]
        private BonusView _bonusViewPrefab;
        [SerializeField]
        private Transform _bonusViewParent;

        private IProcessorDefinition _cachedProcessorDefinition;
        private IBonusProvider _cachedBonusProvider;
        private Action<Guid, IRecipeData> _recipeConfirmedDelegate;
        private RecipeValidationDelegate _processorValidationDelegate;

        private IRecipeSlotFactory _recipeSlotFactory;
        private IBonusViewFactory _bonusViewFactory;

        private readonly List<BonusView> _activeBonusViews = new();

        private void Awake()
        {
            EnsureRecipeSlotFactory();
            EnsureBonusViewFactory();
        }

        private void OnEnable()
        {
            // Creating bonus view OnEnable to account for bonuses changing during runtime
            CreateBonusViews();
        }

        public void SetEnabled(bool enabled)
        {
            gameObject.SetActive(enabled);
        }

        // Could use a builder
        public void Initialize(IProcessorDefinition processorDefinition,
                               IBonusProvider bonusProvider,
                               Action<Guid, IRecipeData> recipeConfirmedDelegate,
                               RecipeValidationDelegate processorValidationDelegate)
        {
            _cachedProcessorDefinition = processorDefinition;
            _cachedBonusProvider = bonusProvider;
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

        private void CreateBonusViews()
        {
            CleanupBonusViews();
            var activeBonuses = GetCachedBonusProvider()?.GetAllActiveBonuses();
            activeBonuses?.ForEach(bonus => GetBonusViewFactory().Create(bonus));
        }

        private void CleanupBonusViews()
        {
            GetActiveBonusViews().ForEach(view => Destroy(view.gameObject));
            GetActiveBonusViews().Clear();
        }

        private void OnRecipeConfirmed(IRecipeData recipeData)
        {
            GetRecipeConfirmedDelegate()?.Invoke(GetCachedProcessorDefinition().GetID(), recipeData);
        }

        private void EnsureRecipeSlotFactory()
        {
            _recipeSlotFactory ??= new DefaultRecipeSlotFactory(GetRecipeSlotPrefab(), GetRecipeSlotParent(), OnRecipeConfirmed);
        }

        private void EnsureBonusViewFactory()
        {
            _bonusViewFactory ??= new DefaultBonusViewFactory(GetBonusViewPrefab(), GetBonusViewParent());
        }

        private CraftingRecipeSlotView GetRecipeSlotPrefab() => _recipeSlotPrefab;
        private Transform GetRecipeSlotParent() => _recipeSlotParent;
        private BonusView GetBonusViewPrefab() => _bonusViewPrefab;
        private Transform GetBonusViewParent() => _bonusViewParent;
        private IProcessorDefinition GetCachedProcessorDefinition() => _cachedProcessorDefinition;
        private IBonusProvider GetCachedBonusProvider() => _cachedBonusProvider;
        private Action<Guid, IRecipeData> GetRecipeConfirmedDelegate() => _recipeConfirmedDelegate;
        private RecipeValidationDelegate GetProcessorValidationDelegate() => _processorValidationDelegate;
        private IRecipeSlotFactory GetRecipeSlotFactory() => _recipeSlotFactory;
        private IBonusViewFactory GetBonusViewFactory() => _bonusViewFactory;
        private List<BonusView> GetActiveBonusViews() => _activeBonusViews;
        
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

        private interface IBonusViewFactory
        {
            BonusView Create(IBonus bonus);
        }

        private class DefaultBonusViewFactory : IBonusViewFactory
        {
            private readonly BonusView _viewPrefab;
            private readonly Transform _viewParent;

            public DefaultBonusViewFactory(BonusView viewPrefab, Transform viewParent)
            {
                _viewPrefab = viewPrefab;
                _viewParent = viewParent;
            }

            public BonusView Create(IBonus bonus)
            {
                var bonusView = Instantiate(_viewPrefab, _viewParent);
                bonusView.Initialize(bonus);
                return bonusView;
            }
        }
    }
}
