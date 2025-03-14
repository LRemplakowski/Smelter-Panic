using System;
using System.Collections.Generic;

namespace SmelterGame.Crafting
{
    public interface IProcessor
    {
        public event CraftingCompletedCallback OnCraftingFinished;

        Guid GetID();
        IReadOnlyCollection<IRecipeData> GetAcceptedRecipes();
        bool CraftRecipe(IRecipeData recipeData, out Func<float> progressDelegate);
        bool CanCraftRecipe(IRecipeData recipe);
    }
}
