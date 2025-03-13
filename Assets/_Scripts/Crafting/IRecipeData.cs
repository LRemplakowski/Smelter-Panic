using System.Collections.Generic;

namespace SmelterGame.Crafting
{
    public interface IRecipeData
    {
        float GetSuccessChance();
        float GetProcessingTime();
        IEnumerable<IProcessable> GetRequiredResources();
        ICraftable GetCraftingResult();
    }
}
