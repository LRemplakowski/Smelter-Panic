using System;
using System.Collections.Generic;

namespace SmelterGame.Crafting
{
    public interface IRecipeData
    {
        Guid GetID();

        string GetName();
        double GetSuccessChance();
        float GetProcessingTime();
        IReadOnlyCollection<CraftingRequirement> GetRequiredResources();
        CraftingYield GetCraftingResult();
    }
}
