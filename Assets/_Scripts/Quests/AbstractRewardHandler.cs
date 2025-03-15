using SmelterGame.Crafting;
using SmelterGame.Inventory;

namespace SmelterGame.Quests
{
    public abstract class AbstractRewardHandler<T> : IRewardHandler
    {
        private IRewardHandler _nextHandler;

        public void HandleReward(IRewardable reward)
        {
            if (reward is T correctReward)
            {
                DoHandleReward(correctReward);
            }
            else
            {
                _nextHandler?.HandleReward(reward);
            }
        }

        protected abstract void DoHandleReward(T correctReward);

        public void SetNext(IRewardHandler nextHandler)
        {
            _nextHandler = nextHandler;
        }
    }

    public class MachineUnlockHandler : AbstractRewardHandler<IMachineUnlockReward>
    {
        private readonly ProcessorManager _processorManager;

        public MachineUnlockHandler(ProcessorManager processorManager)
        {
            _processorManager = processorManager;
        }

        protected override void DoHandleReward(IMachineUnlockReward correctReward)
        {
            _processorManager.UnlockProcessor(correctReward.GetUnlockedProcessor());
        }
    }

    public class ItemRewardHandler : AbstractRewardHandler<IItemReward>
    {
        private readonly IInventory _inventoryManager;

        public ItemRewardHandler(IInventory inventoryManager)
        {
            _inventoryManager = inventoryManager;
        }

        protected override void DoHandleReward(IItemReward correctReward)
        {
            _inventoryManager.AddItem(correctReward.GetRewardedItem());
        }
    }
}
