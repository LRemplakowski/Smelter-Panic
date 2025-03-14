using System.Collections.Generic;

namespace SmelterGame.Inventory
{
    public interface IInventoryPreset
    {
        IReadOnlyCollection<InventoryEntry> GetPresetContents();
    }
}