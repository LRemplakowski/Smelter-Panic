using System.Collections.Generic;
using UnityEngine;

namespace SmelterGame.Quests
{
    [CreateAssetMenu(fileName = "New Complete Quests Quest", menuName = "Quests/Quest Types/Complete Other Quest")]
    public class CompleteOtherQuestsDefinition : AbstractQuestDefinition
    {
        [SerializeField]
        private List<IQuestDefinition> _trackedQuests = new();

        private IQuestFactory _questFactory;

        public IReadOnlyCollection<IQuestDefinition> GetTrackedQuests() => _trackedQuests;

        public override IQuestFactory GetQuestFactory()
        {
            EnsureQuestFactory();
            return _questFactory;
        }

        private void EnsureQuestFactory()
        {
            _questFactory ??= new CompleteOtherQuestsFactory(this);
        }

        private class CompleteOtherQuestsFactory : IQuestFactory
        {
            private readonly CompleteOtherQuestsDefinition _questDefinition;

            public CompleteOtherQuestsFactory(CompleteOtherQuestsDefinition questsDefinition)
            {
                _questDefinition = questsDefinition;
            }

            public IQuest Create()
            {
                return new CompleteOtherQuests(_questDefinition);
            }
        }
    }
}
