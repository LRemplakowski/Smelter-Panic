using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using SmelterGame.Inventory;
using SmelterGame.Inventory.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SmelterGame.Crafting.UI
{
    public class RecipeSlotView : MonoBehaviour, IDropHandler
    {
        [SerializeField]
        private Button _craftRecipeButton;

        [ShowInInspector, ReadOnly]
        private Guid _processorID;
        [ShowInInspector, ReadOnly]
        private IRecipeData _cachedRecipe;
        [ShowInInspector, ReadOnly]
        private ProcessorManager _processorManager;
        [ShowInInspector, ReadOnly]
        private HashSet<IProcessable> _requiredItems;
        [ShowInInspector, ReadOnly]
        private readonly HashSet<Guid> _filledItemIDs = new();

        private void OnDisable()
        {
            _filledItemIDs.Clear();
            _craftRecipeButton.interactable = false;
        }

        public void Initialize(ProcessorManager processorManager, IRecipeData recipe, Guid processorID)
        {
            _processorManager = processorManager;
            _cachedRecipe = recipe;
            _processorID = processorID;
            _requiredItems = recipe.GetRequiredResources().Select(requirement => requirement.ProcessableItem).ToHashSet();
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null && eventData.pointerDrag.TryGetComponent<IItemProvider>(out var itemProvider))
            {
                Debug.Log("Successful drop on Resource Slot");
                UpdateFilledItems(itemProvider);
                UpdateCraftButton();
            }
        }

        private void UpdateCraftButton()
        {
            _craftRecipeButton.interactable = HasAllRequiredItems() && CanProcessorCraftRecipe();
        }

        private bool HasAllRequiredItems()
        {
            return _requiredItems.All(item => _filledItemIDs.Contains(item.GetID()));
        }

        private bool CanProcessorCraftRecipe() => _processorManager.CanCraftRecipe(_processorID, _cachedRecipe);

        private void UpdateFilledItems(IItemProvider itemProvider)
        {
            var providedItem = itemProvider.GetItem();
            if (IsValidItem(providedItem))
            {
                _filledItemIDs.Add(providedItem.GetID());
            }

            bool IsValidItem(IItem providedItem)
            {
                return providedItem != null && _requiredItems.Any(required => required.GetID() == providedItem.GetID());
            }
        }
    }
}
