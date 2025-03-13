namespace SmelterGame.Inventory
{
    public interface IInventory
    {
        bool ContainsItem(IItem item, int count = 1);
        void AddItem(IItem item, int amount);
        bool TryRemoveItem(IItem item, int amount);
    }

    public interface IItem
    {

    }
}
