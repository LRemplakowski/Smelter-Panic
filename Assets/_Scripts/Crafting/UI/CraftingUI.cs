using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SmelterGame.Crafting.UI
{
    public class CraftingUI : SerializedMonoBehaviour
    {
        [Title("Core Config")]
        [SerializeField, Required]
        private ProcessorManager _processorManager;
        [Title("Processor Slots")]
        [SerializeField]
        private ProcessorSlotView _processorSlotPrefab;
        [SerializeField]
        private Transform _processorSlotParent;
        [Title("Processor View")]
        [SerializeField]
        private ProcessorDetailsView _processorViewPrefab;
        [SerializeField]
        private Transform _processorViewParent;

        [Title("Runtime")]
        [ShowInInspector, ReadOnly]
        private ProcessorDetailsView _activeProcessorView;
        [ShowInInspector, ReadOnly]
        private readonly Dictionary<Guid, ProcessorSlotView> _slotViewInstances = new();
        [ShowInInspector, ReadOnly]
        private readonly Dictionary<Guid, ProcessorDetailsView> _processorViewInstances = new();

        private IProcessorSlotFactory _slotFactory;
        private IProcessorViewFactory _viewFactory;

        private void Awake()
        {
            ProcessorManager.OnProcessorUnlocked += OnProcessorUnlocked;
            ProcessorManager.OnCraftingStart += OnCraftingStart;
            ProcessorManager.OnCraftingComplete += OnCraftingComplete;
        }

        private void OnDestroy()
        {
            ProcessorManager.OnProcessorUnlocked -= OnProcessorUnlocked;
            ProcessorManager.OnCraftingStart -= OnCraftingStart;
            ProcessorManager.OnCraftingComplete -= OnCraftingComplete;
        }

        private void OnProcessorUnlocked(IProcessorDefinition processorDefinition)
        {
            EnsureProcessorSlotFactory();
            EnsureProcessorViewFactory();
            var processorID = processorDefinition.GetID();
            var slotViewInstance = GetProcessorSlotFactory().Create(processorDefinition);
            _slotViewInstances[processorID] = slotViewInstance;
            var processorViewInstance = GetProcessorViewFactory().Create(processorDefinition);
            _processorViewInstances[processorID] = processorViewInstance;
            processorViewInstance.SetEnabled(false);
        }

        private void OnCraftingComplete(in Guid processorID, bool isSuccess, CraftingYield result)
        {
            if (_slotViewInstances.TryGetValue(processorID, out var slotView))
            {
                slotView.SetSlotInteractable(true);
                slotView.GetProgressBar().StopProgressBar();
            }
        }

        private void OnCraftingStart(in Guid processorID, IRecipeData recipe, in Func<float> progressDelegate)
        {
            HideProcessorView();
            if (_slotViewInstances.TryGetValue(processorID, out var slotView))
            {
                slotView.SetSlotInteractable(false);
                slotView.GetProgressBar().StartProgressBar(recipe.GetName(), progressDelegate);
            }
        }

        private void OnSlotClicked(Guid processorID)
        {
            ShowProcessorView(processorID);
        }

        private void OnRecipeConfirmed(Guid processorID, IRecipeData recipe)
        {
            GetProcessorManager().RunRecipe(processorID, recipe);
        }

        private void ShowProcessorView(Guid processorID)
        {
            HideProcessorView();
            if (_processorViewInstances.TryGetValue(processorID, out _activeProcessorView))
            {
                _activeProcessorView.SetEnabled(true);
            }
        }

        private void HideProcessorView()
        {
            if (_activeProcessorView != null)
            {
                _activeProcessorView.SetEnabled(false);
                _activeProcessorView = null;
            }
        }

        private void EnsureProcessorSlotFactory()
        {
            _slotFactory ??= new DefaultSlotFactory(GetProcessorSlotPrefab(), GetProcessorSlotParent(), OnSlotClicked);
        }

        private void EnsureProcessorViewFactory()
        {
            _viewFactory ??= new DefaultViewFactory(GetProcessorViewPrefab(),
                                                    GetProcessorViewParent(),
                                                    OnRecipeConfirmed,
                                                    GetProcessorManager().CanCraftRecipe);
        }

        private ProcessorSlotView GetProcessorSlotPrefab() => _processorSlotPrefab;
        private Transform GetProcessorSlotParent() => _processorSlotParent;
        private ProcessorDetailsView GetProcessorViewPrefab() => _processorViewPrefab;
        private Transform GetProcessorViewParent() => _processorViewParent;
        private IProcessorSlotFactory GetProcessorSlotFactory() => _slotFactory;
        private IProcessorViewFactory GetProcessorViewFactory() => _viewFactory;
        private ProcessorManager GetProcessorManager() => _processorManager;

        private interface IProcessorViewFactory
        {
            ProcessorDetailsView Create(IProcessorDefinition processorDefinition);
        }

        private class DefaultViewFactory : IProcessorViewFactory
        {
            private readonly ProcessorDetailsView _viewPrefab;
            private readonly Transform _viewParent;
            private readonly Action<Guid, IRecipeData> _recipeConfirmedDelegate;
            private readonly RecipeValidationDelegate _processorConfirmationDelegate;

            // Could use a builder
            public DefaultViewFactory(ProcessorDetailsView viewPrefab,
                                      Transform viewParent,
                                      Action<Guid, IRecipeData> recipeConfirmedDelegate,
                                      RecipeValidationDelegate processorConfirmationDelegate)
            {
                _viewPrefab = viewPrefab;
                _viewParent = viewParent;
                _recipeConfirmedDelegate = recipeConfirmedDelegate;
                _processorConfirmationDelegate = processorConfirmationDelegate;
            }

            public ProcessorDetailsView Create(IProcessorDefinition processorDefinition)
            {
                var viewInstance = Instantiate(_viewPrefab, _viewParent);
                viewInstance.Initialize(processorDefinition, _recipeConfirmedDelegate, _processorConfirmationDelegate);
                return viewInstance;
            }
        }

        private interface IProcessorSlotFactory
        {
            ProcessorSlotView Create(IProcessorDefinition processorDefinition);
        }

        private class DefaultSlotFactory : IProcessorSlotFactory
        {
            private readonly ProcessorSlotView _slotPrefab;
            private readonly Transform _slotParent;
            private readonly Action<Guid> _slotClickedDelegate;

            public DefaultSlotFactory(ProcessorSlotView slotPrefab, Transform slotParent, Action<Guid> slotClickedDelegate)
            {
                _slotPrefab = slotPrefab;
                _slotParent = slotParent;
                _slotClickedDelegate = slotClickedDelegate;
            }

            public ProcessorSlotView Create(IProcessorDefinition processorDefinition)
            {
                ProcessorSlotView slotView = Instantiate(_slotPrefab, _slotParent);
                slotView.Initialize(processorDefinition, _slotClickedDelegate);
                return slotView;
            }
        }
    }
}
