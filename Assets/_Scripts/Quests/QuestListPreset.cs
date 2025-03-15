using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SmelterGame.Quests
{
    [CreateAssetMenu(fileName = "New Quest List Preset", menuName = "Quests/Presets/List Preset")]
    public class QuestListPreset : SerializedScriptableObject, IQuestPreset
    {
        [SerializeField]
        private List<IQuestDefinition> _quests = new();

        public IReadOnlyCollection<IQuestDefinition> GetQuests() => _quests;
    }
}
