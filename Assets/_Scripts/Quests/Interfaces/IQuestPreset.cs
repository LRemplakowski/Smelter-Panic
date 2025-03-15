using System.Collections.Generic;

namespace SmelterGame.Quests
{
    public interface IQuestPreset
    {
        IReadOnlyCollection<IQuestDefinition> GetQuests();
    }

}
