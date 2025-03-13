using System;
using System.Collections.Generic;
using System.Linq;
using SmelterGame.Inventory;

namespace SmelterGame.Crafting
{
    public class CraftingProcessor : IProcessor
    {
        private readonly IProcessorDefinition _definition;
        private readonly IInventory _resourceInventory;

        public CraftingProcessor(IProcessorDefinition definition, IInventory resourceInventory)
        {
            _definition = definition;
            _resourceInventory = resourceInventory;
        }

        public bool CraftRecipe(IRecipeData recipeData, out Func<float> progressDelegate)
        {
            progressDelegate = default;
            if (IsValidRecipe(recipeData) && HasRequiredResources(_resourceInventory, recipeData))
            {
                var craftingProcess = new CraftingProcess(recipeData.GetProcessingTime(), recipeData.GetCraftingResult());
                progressDelegate = craftingProcess.GetProgress;
                craftingProcess.Begin(OnCraftingFinished);
                return true;
            }
            return false;
        }

        public ICollection<IRecipeData> GetRecipes()
        {
            return _definition.GetAcceptedRecipes();
        }

        private void OnCraftingFinished(ICraftable result)
        {

        }

        private static bool HasRequiredResources(IInventory inventory, IRecipeData recipe)
        {
            var requiredResources = recipe.GetRequiredResources();
            return requiredResources.All(resource => resource is IItem item && inventory.ContainsItem(item));
        }

        private bool IsValidRecipe(IRecipeData recipe)
        {
            return _definition.GetAcceptedRecipes().Contains(recipe);
        }
    }
}
