using Sirenix.Utilities;
using UnityEngine;

namespace SmelterGame.Quests
{
    public delegate void QuestUpdateDelegate(IQuest quest);

    public class QuestManager : MonoBehaviour
    {
        [SerializeField]
        private IQuestPreset _startingQuestsPreset;

        public static event QuestUpdateDelegate OnQuestUpdated;
        public static event QuestUpdateDelegate OnQuestCompleted;

        private void Start()
        {
            GetStartingQuestsPreset()?.GetQuests().ForEach(questDefinition => StartQuest(questDefinition));
        }

        public void StartQuest(IQuestDefinition questDefinition)
        {
            var quest = questDefinition.GetQuestFactory().Create();
            quest.Initialize();
            quest.OnQuestProgressUpdated += QuestUpdated;
        }

        private void QuestUpdated(IQuest quest)
        {
            OnQuestUpdated?.Invoke(quest);
            if (quest.EvaluateCompleted())
            {
                QuestCompleted(quest);
            }
        }

        private void QuestCompleted(IQuest quest)
        {
            OnQuestCompleted?.Invoke(quest);
            quest.OnQuestProgressUpdated -= QuestUpdated;
            quest.Cleanup();
        }

        private IQuestPreset GetStartingQuestsPreset() => _startingQuestsPreset;
    }
}
