using System;

namespace SmelterGame.Inventory
{
    public interface IItem
    {
        Guid GetID();

        string GetName();
        string GetDescription();
    }
}
