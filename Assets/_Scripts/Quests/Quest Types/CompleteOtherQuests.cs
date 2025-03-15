using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SmelterGame.Quests
{
    public class CompleteOtherQuests : AbstractQuest
    {
        public override event Action<IQuest> OnQuestProgressUpdated;

        private readonly CompleteOtherQuestsDefinition _questDefinition;

        private readonly HashSet<Guid> _completedQuestIDs = new();

        public CompleteOtherQuests(CompleteOtherQuestsDefinition questDefinition) : base(questDefinition)
        {
            _questDefinition = questDefinition;
        }

        public override void Initialize()
        {
            QuestManager.OnQuestCompleted += OnTrackedQuestCompleted;
        }

        public override void Cleanup()
        {
            QuestManager.OnQuestCompleted -= OnTrackedQuestCompleted;
        }

        private void OnTrackedQuestCompleted(IQuest quest)
        {
            if (_questDefinition.GetTrackedQuests().Any(tracked => tracked.GetID() == quest.GetID()))
            {
                _completedQuestIDs.Add(quest.GetID());
                OnQuestProgressUpdated?.Invoke(this);
            }
        }

        public override bool EvaluateCompleted()
        {
            return _questDefinition.GetTrackedQuests().All(tracked => _completedQuestIDs.Contains(tracked.GetID()));
        }

        public override string GetProgressText()
        {
            return $"Completed quests: {GetQuestNames()}";
        }

        private string GetQuestNames()
        {
            return string.Join(", ", _questDefinition.GetTrackedQuests().Select(tracked => tracked.GetName()));
        }
    }
}
