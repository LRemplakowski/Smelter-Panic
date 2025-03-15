using Sirenix.OdinInspector;
using Sirenix.Utilities;
using SmelterGame.Crafting;
using SmelterGame.Inventory;
using UnityEngine;

namespace SmelterGame.Quests
{
    public delegate void QuestUpdateDelegate(IQuest quest);

    public partial class QuestManager : SerializedMonoBehaviour
    {
        [SerializeField]
        private IQuestPreset _startingQuestsPreset;
        [SerializeField]
        private ProcessorManager _processorManager;
        [SerializeField]
        private InventoryManager _inventoryManager;

        public static event QuestUpdateDelegate OnQuestStarted;
        public static event QuestUpdateDelegate OnQuestUpdated;
        public static event QuestUpdateDelegate OnQuestCompleted;

        private IRewardHandler _rewardHandlerChain;

        private void Start()
        {
            GetStartingQuestsPreset()?.GetQuests().ForEach(questDefinition => StartQuest(questDefinition));
            CreateRewardHandlerChain();
        }

        public void StartQuest(IQuestDefinition questDefinition)
        {
            var quest = questDefinition.GetQuestFactory().Create();
            quest.Initialize();
            quest.OnQuestProgressUpdated += QuestUpdated;
            OnQuestStarted?.Invoke(quest);
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
            ApplyReward(quest);
            DoQuestCleanup(quest);
            StartNextQuest(quest);

            void ApplyReward(IQuest quest)
            {
                var questReward = quest.GetReward();
                if (questReward != null)
                {
                    GetRewardHandlerChain().HandleReward(questReward);
                }
            }

            void StartNextQuest(IQuest quest)
            {
                var nextQuest = quest.GetNextQuest();
                if (nextQuest != null)
                {
                    StartQuest(nextQuest);
                }
            }

            void DoQuestCleanup(IQuest quest)
            {
                quest.OnQuestProgressUpdated -= QuestUpdated;
                quest.Cleanup();
            }
        }

        private void CreateRewardHandlerChain()
        {
            var machineUnlock = new MachineUnlockHandler(GetProcessorManager());
            var itemReward = new ItemRewardHandler(GetInventoryManager());
            machineUnlock.SetNext(itemReward);
            _rewardHandlerChain = machineUnlock;
        }

        private IQuestPreset GetStartingQuestsPreset() => _startingQuestsPreset;
        private ProcessorManager GetProcessorManager() => _processorManager;
        private IInventory GetInventoryManager() => _inventoryManager;
        private IRewardHandler GetRewardHandlerChain() => _rewardHandlerChain;
    }
}
