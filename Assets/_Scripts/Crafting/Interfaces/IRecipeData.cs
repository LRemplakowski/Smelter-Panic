using System;
using System.Collections.Generic;

namespace SmelterGame.Crafting
{
    public interface IRecipeData
    {
        Guid GetID();

        string GetName();
        float GetSuccessChance();
        float GetProcessingTime();
        IReadOnlyCollection<CraftingRequirement> GetRequiredResources();
        CraftingYield GetCraftingResult();
    }
}
