using System;

namespace SmelterGame.Quests
{
    public interface IQuest
    {
        public event Action<IQuest> OnQuestProgressUpdated;

        Guid GetID();
        string GetName();
        string GetDescription();
        IQuestDefinition GetNextQuest();
        IRewardable GetReward();
        bool EvaluateCompleted();
        string GetProgressText();
        void Initialize();
        void Cleanup();
    }
}
