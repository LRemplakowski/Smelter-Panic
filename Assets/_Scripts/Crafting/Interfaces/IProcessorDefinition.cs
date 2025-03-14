using System;
using System.Collections.Generic;

namespace SmelterGame.Crafting
{
    public interface IProcessorDefinition
    {
        Guid GetID();
        IReadOnlyCollection<IRecipeData> GetAcceptedRecipes();
    }
}
