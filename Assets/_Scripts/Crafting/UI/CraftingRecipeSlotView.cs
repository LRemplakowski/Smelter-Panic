using System;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using SmelterGame.Inventory;
using SmelterGame.Inventory.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using Sirenix.Utilities;

namespace SmelterGame.Crafting.UI
{
    public class CraftingRecipeSlotView : MonoBehaviour, IDropHandler
    {
        [SerializeField]
        private Image _recipeYieldIcon;
        [SerializeField]
        private TextMeshProUGUI _recipeName;
        [SerializeField]
        private ResourceIcon _resourceIconPrefab;
        [SerializeField]
        private Transform _resourceIconParent;
        [SerializeField]
        private Button _craftRecipeButton;

        [ShowInInspector, ReadOnly]
        private Guid _processorID;
        [ShowInInspector, ReadOnly]
        private HashSet<IProcessable> _requiredItems;
        [ShowInInspector, ReadOnly]
        private readonly HashSet<Guid> _filledItemIDs = new();
        private readonly Dictionary<Guid, ResourceIcon> _resourceIcons = new();

        private Action<IRecipeData> _craftingConfirmedDelegate;
        private RecipeValidationDelegate _processorValidationDelegate;
        private IRecipeData _cachedRecipe;
        private IResourceIconFactory _iconFactory;

        private void Awake()
        {
            EnsureIconFactory();
        }

        private void OnDisable()
        {
            GetFilledItemIDs().Clear();
            UpdateCraftButton();
            UpdateResourceIcons();
        }

        // Could use a builder
        public void Initialize(Guid processorID,
                               IRecipeData recipeData,
                               Action<IRecipeData> craftingConfirmedDelegate,
                               RecipeValidationDelegate processorValidationDelegate)
        {
            SetReferences(processorID, recipeData, craftingConfirmedDelegate, processorValidationDelegate);
            CreateResourceIcons();

            void CreateResourceIcons()
            {
                var requiredItems = GetRequiredItems();
                foreach (var item in requiredItems)
                {
                    var iconInstance = GetResourceIconFactory().Create(item.GetIcon());
                    iconInstance.SetActive(false);
                    GetResourceIconMap()[item.GetID()] = iconInstance;
                }
            }

            void SetReferences(Guid processorID, IRecipeData recipeData, Action<IRecipeData> craftingConfirmedDelegate, RecipeValidationDelegate processorValidationDelegate)
            {
                _processorID = processorID;
                _craftingConfirmedDelegate = craftingConfirmedDelegate;
                _processorValidationDelegate = processorValidationDelegate;
                _cachedRecipe = recipeData;
                _recipeYieldIcon.sprite = recipeData.GetCraftingResult().YieldItem.GetIcon();
                _recipeName.text = recipeData.GetName();
                _requiredItems = recipeData.GetRequiredResources().Select(requirement => requirement.ProcessableItem).ToHashSet();
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null && eventData.pointerDrag.TryGetComponent<IItemProvider>(out var itemProvider))
            {
                Debug.Log("Successful drop on Resource Slot");
                UpdateFilledItems(itemProvider);
                UpdateCraftButton();
                UpdateResourceIcons();
            }
        }

        public void OnRecipeConfirmed()
        {
            GetCraftingConfirmedDelegate()?.Invoke(GetCachedRecipe());
        }

        private void UpdateCraftButton()
        {
            GetCraftRecipeButton().interactable = HasAllRequiredItems() && CanProcessorCraftRecipe();
        }

        private void UpdateResourceIcons()
        {
            foreach (var item in GetRequiredItems())
            {
                if (GetResourceIconMap().TryGetValue(item.GetID(), out var icon))
                {
                    icon.SetActive(GetFilledItemIDs().Contains(item.GetID()));
                }
            }
        }

        private bool HasAllRequiredItems() => GetRequiredItems().All(item => GetFilledItemIDs().Contains(item.GetID()));
        private bool CanProcessorCraftRecipe() => GetProcessorValidationDelegate()?.Invoke(GetProcessorID(), GetCachedRecipe()) ?? false;

        private void UpdateFilledItems(IItemProvider itemProvider)
        {
            var providedItem = itemProvider.GetItem();
            if (IsValidItem(providedItem))
            {
                GetFilledItemIDs().Add(providedItem.GetID());
            }

            bool IsValidItem(IItem providedItem)
            {
                return providedItem != null && GetRequiredItems().Any(required => required.GetID() == providedItem.GetID());
            }
        }

        private void EnsureIconFactory()
        {
            _iconFactory ??= new DefaultResourceIconFactory(GetResourceIconPrefab(), GetResourceIconParent());
        }

        private Button GetCraftRecipeButton() => _craftRecipeButton;
        private IRecipeData GetCachedRecipe() => _cachedRecipe;
        private Guid GetProcessorID() => _processorID;
        private HashSet<IProcessable> GetRequiredItems() => _requiredItems;
        private HashSet<Guid> GetFilledItemIDs() => _filledItemIDs;
        private Dictionary<Guid, ResourceIcon> GetResourceIconMap() => _resourceIcons;
        private ResourceIcon GetResourceIconPrefab() => _resourceIconPrefab;
        private Transform GetResourceIconParent() => _resourceIconParent;
        private IResourceIconFactory GetResourceIconFactory() => _iconFactory;
        private Action<IRecipeData> GetCraftingConfirmedDelegate() => _craftingConfirmedDelegate;
        private RecipeValidationDelegate GetProcessorValidationDelegate() => _processorValidationDelegate;

        private interface IResourceIconFactory
        {
            ResourceIcon Create(Sprite icon);
        }

        private class DefaultResourceIconFactory : IResourceIconFactory
        {
            private readonly ResourceIcon _iconPrefab;
            private readonly Transform _iconParent;

            public DefaultResourceIconFactory(ResourceIcon iconPrefab, Transform iconParent)
            {
                _iconPrefab = iconPrefab;
                _iconParent = iconParent;
            }

            public ResourceIcon Create(Sprite icon)
            {
                var instance = Instantiate(_iconPrefab, _iconParent);
                instance.Initialize(icon);
                return instance;
            }
        }
    }
}
