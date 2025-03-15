using System;

namespace SmelterGame.Quests
{
    public interface IQuestDefinition
    {
        Guid GetID();
        string GetName();
        string GetDescription();
        bool IsHidden();
        IQuestDefinition GetNextQuest();
        IRewardable GetReward();
        IQuestFactory GetQuestFactory();
    }
}
