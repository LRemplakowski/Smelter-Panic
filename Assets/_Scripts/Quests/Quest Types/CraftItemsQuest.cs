using System;
using SmelterGame.Crafting;
using SmelterGame.Inventory;

namespace SmelterGame.Quests
{
    public class CraftItemsQuest : IQuest
    {
        public event Action<IQuest> OnQuestProgressUpdated;

        private readonly CraftItemsQuestDefinition _questDefinition;

        private int _currentCraftedAmount = 0;

        public CraftItemsQuest(CraftItemsQuestDefinition questDefinition)
        {
            _questDefinition = questDefinition;
        }

        public Guid GetID() => _questDefinition.GetID();
        public string GetName() => _questDefinition.GetName();
        public string GetDescription() => _questDefinition.GetDescription();
        public IQuestDefinition GetNextQuest() => _questDefinition.GetNextQuest();
        public IRewardable GetReward() => _questDefinition.GetReward();

        public bool EvaluateCompleted()
        {
            return _currentCraftedAmount >= _questDefinition.GetRequiredAmount();
        }

        public string GetProgressText()
        {
            return $"Craft {_questDefinition.GetTrackedItem().GetName()}: {_currentCraftedAmount}/{_questDefinition.GetRequiredAmount()}";
        }

        public void Initialize()
        {
            ProcessorManager.OnCraftingComplete += OnItemCrafted;
        }

        public void Cleanup()
        {
            ProcessorManager.OnCraftingComplete -= OnItemCrafted;
        }

        private void OnItemCrafted(in Guid _, bool isSuccess, CraftingYield result)
        {
            if (isSuccess && result.YieldItem.GetID() == _questDefinition.GetTrackedItem().GetID())
            {
                _currentCraftedAmount += result.YieldAmount;
                OnQuestProgressUpdated?.Invoke(this);
            }
        }
    }
}
