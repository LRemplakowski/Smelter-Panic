using System;
using SmelterGame.Crafting;
using SmelterGame.Inventory;

namespace SmelterGame.Quests
{
    public class CraftItemsQuest : IQuest
    {
        public event Action<IQuest> OnQuestProgressUpdated;

        private readonly Guid _questID;
        private readonly IItem _trackedItem;
        private readonly int _requiredAmount;
        private readonly IQuestDefinition _nextQuest;

        private int _currentCraftedAmount = 0;

        public CraftItemsQuest(Guid questID, IItem trackedItem, int requiredAmount, IQuestDefinition nextQuest)
        {
            _questID = questID;
            _trackedItem = trackedItem;
            _requiredAmount = requiredAmount;
            _nextQuest = nextQuest;
        }

        public Guid GetID() => _questID;
        public IQuestDefinition GetNextQuest() => _nextQuest;

        public bool EvaluateCompleted()
        {
            return _currentCraftedAmount >= _requiredAmount;
        }

        public string GetProgressText()
        {
            return $"Craft {_trackedItem.GetName()}: {_currentCraftedAmount}/{_requiredAmount}";
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
            if (isSuccess && result.YieldItem.GetID() == _trackedItem.GetID())
            {
                _currentCraftedAmount += result.YieldAmount;
                OnQuestProgressUpdated?.Invoke(this);
            }
        }
    }
}
