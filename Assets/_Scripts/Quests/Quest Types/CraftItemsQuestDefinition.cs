using System;
using Sirenix.OdinInspector;
using SmelterGame.Inventory;
using UnityEngine;

namespace SmelterGame.Quests
{
    [CreateAssetMenu(fileName = "New Craft Items Quest", menuName = "Quests/Craft Items Quest")]
    public class CraftItemsQuestDefinition : SerializedScriptableObject, IQuestDefinition
    {
        [SerializeField]
        private Guid _questID = Guid.NewGuid();
        [SerializeField]
        private string _name;
        [SerializeField, MultiLineProperty]
        private string _description;
        [SerializeField, Required]
        private IItem _trackedItem;
        [SerializeField, MinValue(1)]
        private int _requiredAmount = 1;
        [SerializeField]
        private IQuestDefinition _nextQuest;
        [SerializeField]
        private IRewardable _questReward;

        private IQuestFactory _questFactory;

        public Guid GetID() => _questID;
        public string GetName() => _name;
        public string GetDescription() => _description;

        public IQuestFactory GetQuestFactory()
        {
            EnsureQuestFactory();
            return _questFactory;
        }

        public IItem GetTrackedItem() => _trackedItem;
        public int GetRequiredAmount() => _requiredAmount;
        public IQuestDefinition GetNextQuest() => _nextQuest;
        public IRewardable GetReward() => _questReward;

        private void EnsureQuestFactory()
        {
            _questFactory ??= new CraftItemsQuestFactory(this);
        }

        private class CraftItemsQuestFactory : IQuestFactory
        {
            private CraftItemsQuestDefinition _questDefinition;

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
