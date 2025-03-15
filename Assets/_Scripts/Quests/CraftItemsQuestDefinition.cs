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
        [SerializeField, Required]
        private IItem _trackedItem;
        [SerializeField, MinValue(1)]
        private int _requiredAmount;
        [SerializeField]
        private IQuestDefinition _nextQuest;

        private IQuestFactory _questFactory;

        public Guid GetID() => _questID;

        public IQuestFactory GetQuestFactory()
        {
            EnsureQuestFactory();
            return _questFactory;
        }

        private void EnsureQuestFactory()
        {
            _questFactory ??= new CraftItemsQuestFactory(this);
        }

        private class CraftItemsQuestFactory : IQuestFactory
        {
            private readonly Guid _questID;
            private readonly IItem _trackedItem;
            private readonly int _requiredAmount;
            private readonly IQuestDefinition _nextQuest;

            public CraftItemsQuestFactory(CraftItemsQuestDefinition questDefinition)
            {
                _questID = questDefinition.GetID();
                _trackedItem = questDefinition._trackedItem;
                _requiredAmount = questDefinition._requiredAmount;
                _nextQuest = questDefinition._nextQuest;
            }

            public IQuest Create()
            {
                var quest = new CraftItemsQuest(_questID, _trackedItem, _requiredAmount, _nextQuest);
                return quest;
            }
        }
    }
}
