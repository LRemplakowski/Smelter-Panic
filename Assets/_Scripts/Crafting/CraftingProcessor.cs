using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using SmelterGame.Bonuses;
using SmelterGame.Inventory;
using UnityEngine;

namespace SmelterGame.Crafting
{
    public class CraftingProcessor : IProcessor
    {
        public event CraftingCompletedCallback OnCraftingFinished;

        private readonly Guid _processorID;
        private readonly IProcessorDefinition _definition;
        private readonly IInventory _resourceInventory;
        private readonly IBonusProvider _bonusProvider;

        public CraftingProcessor(IProcessorDefinition definition, IInventory resourceInventory, IBonusProvider bonusProvider)
        {
            _definition = definition;
            _processorID = definition.GetID();
            _resourceInventory = resourceInventory;
            _bonusProvider = bonusProvider;
        }

        public Guid GetID() => _processorID;
        public IReadOnlyCollection<IRecipeData> GetAcceptedRecipes() => _definition.GetAcceptedRecipes();

        public bool CraftRecipe(IRecipeData recipeData, out Func<float> progressDelegate)
        {
            progressDelegate = default;
            bool isRecipeAcceptedByProcessor = IsValidRecipe(GetAcceptedRecipes(), recipeData);
            if (isRecipeAcceptedByProcessor)
            {
                if (!ConsumeResources(_resourceInventory, recipeData))
                {
                    return false;
                }
                var craftingProcess = new CraftingProcess(GetID(), recipeData.GetProcessingTime(), recipeData.GetSuccessChance(), recipeData.GetCraftingResult(), _bonusProvider);
                progressDelegate = craftingProcess.GetProgress;
                craftingProcess.Begin(CraftingFinished);
                return true;
            }
            Debug.LogError($"{nameof(CraftingProcessor)} >>> Processor {_definition} ordered to craft invalid recipe {recipeData}! Processor ID: {_processorID}");
            return false;
        }

        public bool CanCraftRecipe(IRecipeData recipe)
        {
            return IsValidRecipe(GetAcceptedRecipes(), recipe) && HasAllRequiredResourcesInInventory(recipe, _resourceInventory);
        }

        private void CraftingFinished(in Guid processorID, bool isSuccess, CraftingYield result)
        {
            if (isSuccess)
            {
                AddCraftingYieldToInventory(_resourceInventory, result);
            }
            OnCraftingFinished?.Invoke(in processorID, isSuccess, result);
        }

        private static bool ConsumeResources(IInventory inventory, IRecipeData recipe)
        {
            var requiredResourecs = recipe.GetRequiredResources();
            bool result = false;
            using (TransactionScope transaction = new())
            {
                result = requiredResourecs.All(resource => inventory.TryRemoveItem(resource.ProcessableItem, resource.RequiredAmount));
                if (result)
                {
                    transaction.Complete();
                }
            }
            return result;
        }

        private static bool HasAllRequiredResourcesInInventory(IRecipeData recipe, IInventory inventory)
        {
            return recipe.GetRequiredResources().All(resource => inventory.ContainsItem(resource.ProcessableItem, resource.RequiredAmount));
        }

        private static void AddCraftingYieldToInventory(IInventory inventory, CraftingYield craftingYield)
        {
            inventory.AddItem(craftingYield.YieldItem, craftingYield.YieldAmount);
        }

        private static bool IsValidRecipe(IEnumerable<IRecipeData> acceptedRecipes, IRecipeData recipe)
        {
            return acceptedRecipes.Any(accepted => recipe.GetID() == accepted.GetID());
        }
    }
}
