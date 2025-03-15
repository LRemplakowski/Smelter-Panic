using System;

namespace SmelterGame.Quests
{
    public interface IQuestDefinition
    {
        Guid GetID();
        string GetName();
        string GetDescription();
        IRewardable GetReward();
        IQuestFactory GetQuestFactory();
    }
}
