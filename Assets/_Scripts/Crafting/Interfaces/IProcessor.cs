using System;
using System.Collections.Generic;

namespace SmelterGame.Crafting
{
    public interface IProcessor
    {
        Guid GetID();
        IReadOnlyCollection<IRecipeData> GetAcceptedRecipes();
        bool CraftRecipe(IRecipeData recipeData, out Func<float> progressDelegate);
    }
}
