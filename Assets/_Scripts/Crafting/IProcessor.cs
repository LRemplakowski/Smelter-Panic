using System;
using System.Collections.Generic;

namespace SmelterGame.Crafting
{
    public interface IProcessor
    {
        bool CraftRecipe(IRecipeData recipeData, out Func<float> progressDelegate);

        ICollection<IRecipeData> GetRecipes();
    }
}
