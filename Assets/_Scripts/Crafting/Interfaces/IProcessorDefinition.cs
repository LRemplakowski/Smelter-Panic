using System;
using System.Collections.Generic;
using UnityEngine;

namespace SmelterGame.Crafting
{
    public interface IProcessorDefinition
    {
        Guid GetID();
        string GetName();
        Sprite GetIcon();
        IReadOnlyCollection<IRecipeData> GetAcceptedRecipes();
    }
}
