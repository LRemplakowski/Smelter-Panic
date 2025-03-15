using System;

namespace SmelterGame.Quests
{
    public interface IQuestDefinition
    {
        Guid GetID();
        IQuestFactory GetQuestFactory();
    }
}
