using System;
using Sirenix.OdinInspector;
using SmelterGame.Inventory;
using UnityEngine;

namespace SmelterGame.Quests
{
    [CreateAssetMenu(fileName = "New Craft Items Quest", menuName = "Quests/Quest Types/Craft Items Quest")]
    public class CraftItemsQuestDefinition : AbstractQuestDefinition
    {
        [SerializeField, Required]
        private IItem _trackedItem;
        [SerializeField, MinValue(1)]
        private int _requiredAmount = 1;

        private IQuestFactory _questFactory;

        public override IQuestFactory GetQuestFactory()
        {
            EnsureQuestFactory();
            return _questFactory;
        }

        public IItem GetTrackedItem() => _trackedItem;
        public int GetRequiredAmount() => _requiredAmount;

        private void EnsureQuestFactory()
        {
            _questFactory ??= new CraftItemsQuestFactory(this);
        }

        private class CraftItemsQuestFactory : IQuestFactory
        {
            private readonly CraftItemsQuestDefinition _questDefinition;

            public CraftItemsQuestFactory(CraftItemsQuestDefinition questDefinition)
            {
                _questDefinition = questDefinition;
            }

            public IQuest Create()
            {
                return new CraftItemsQuest(_questDefinition);
            }
        }
    }
}
