using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SmelterGame.Crafting.UI
{
    public class ProcessorListUI : SerializedMonoBehaviour
    {
        //[SerializeField]
        //private ProcessorManager _processorManager;
        [SerializeField]
        private ProcessorSlotView _processorSlotPrefab;
        [SerializeField]
        private Transform _processorSlotParent;

        private readonly Dictionary<Guid, ProcessorSlotView> _slotViewInstances = new();
        private IProcessorSlotFactory _slotFactory;

        private void Awake()
        {
            EnsureSlotFactory();
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
            var viewInstance = GetProcessorSlotFactory().Create(processorDefinition);
            _slotViewInstances[processorDefinition.GetID()] = viewInstance;
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
            if (_slotViewInstances.TryGetValue(processorID, out var slotView))
            {
                slotView.SetSlotInteractable(false);
                slotView.GetProgressBar().StartProgressBar(recipe.GetName(), progressDelegate);
            }
        }

        private void OnSlotClicked(Guid id)
        {

        }

        private void EnsureSlotFactory()
        {
            _slotFactory = new DefaultSlotFactory(_processorSlotPrefab, _processorSlotParent, OnSlotClicked);
        }

        private IProcessorSlotFactory GetProcessorSlotFactory() => _slotFactory;

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
