using System.Collections.Generic;

namespace SmelterGame.Crafting
{
    public interface IProcessorDefinition
    {
        ICollection<IRecipeData> GetAcceptedRecipes();
    }
}
