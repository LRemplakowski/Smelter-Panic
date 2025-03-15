using Sirenix.OdinInspector;
using SmelterGame.Inventory;

namespace SmelterGame.Quests
{
    public abstract class AbstractItemReward : SerializedScriptableObject, IRewardable, IItemReward
    {
        public abstract IItem GetRewardedItem();
    }

    public interface IItemReward : IRewardable
    {
        IItem GetRewardedItem();
    }
}