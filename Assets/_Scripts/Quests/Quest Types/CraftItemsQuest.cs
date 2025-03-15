using System;
using SmelterGame.Crafting;

namespace SmelterGame.Quests
{
    public class CraftItemsQuest : AbstractQuest
    {
        public override event Action<IQuest> OnQuestProgressUpdated;

        private readonly CraftItemsQuestDefinition _questDefinition;

        private int _currentCraftedAmount = 0;

        public CraftItemsQuest(CraftItemsQuestDefinition questDefinition) : base(questDefinition)
        {
            _questDefinition = questDefinition;
        }

        public override bool EvaluateCompleted()
        {
            return _currentCraftedAmount >= _questDefinition.GetRequiredAmount();
        }

        public override string GetProgressText()
        {
            return $"Craft {_questDefinition.GetTrackedItem().GetName()}: {_currentCraftedAmount}/{_questDefinition.GetRequiredAmount()}";
        }

        public override void Initialize()
        {
            ProcessorManager.OnCraftingComplete += OnItemCrafted;
        }

        public override void Cleanup()
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
