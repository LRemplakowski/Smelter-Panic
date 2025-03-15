using System;

namespace SmelterGame.Quests
{
    public abstract class AbstractQuest : IQuest
    {
        public abstract event Action<IQuest> OnQuestProgressUpdated;

        private readonly IQuestDefinition _questDefinition;

        public AbstractQuest(IQuestDefinition questDefinition)
        {
            _questDefinition = questDefinition;
        }

        public Guid GetID() => _questDefinition.GetID();
        public string GetName() => _questDefinition.GetName();
        public string GetDescription() => _questDefinition.GetDescription();
        public bool IsHidden() => _questDefinition.IsHidden();
        public IQuestDefinition GetNextQuest() => _questDefinition.GetNextQuest();
        public IRewardable GetReward() => _questDefinition.GetReward();

        public abstract void Initialize();
        public abstract void Cleanup();
        public abstract string GetProgressText();
        public abstract bool EvaluateCompleted();
    }
}