using System;

namespace SmelterGame.Quests
{
    public interface IQuest
    {
        public event Action<IQuest> OnQuestProgressUpdated;

        Guid GetID();
        IQuestDefinition GetNextQuest();
        bool EvaluateCompleted();
        string GetProgressText();
        void Initialize();
        void Cleanup();
    }
}
