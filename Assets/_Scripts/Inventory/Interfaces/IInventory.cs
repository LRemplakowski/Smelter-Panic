namespace SmelterGame.Inventory
{
    public interface IInventory
    {
        bool ContainsItem(IItem item, int count = 1);
        void AddItem(IItem item, int amount = 1);
        bool TryRemoveItem(IItem item, int amount = 1);
    }
}
