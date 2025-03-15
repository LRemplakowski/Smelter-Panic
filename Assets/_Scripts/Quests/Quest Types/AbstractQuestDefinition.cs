using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SmelterGame.Quests
{
    public abstract class AbstractQuestDefinition : SerializedScriptableObject, IQuestDefinition
    {
        [SerializeField]
        private Guid _questID = Guid.NewGuid();
        [SerializeField]
        private string _name;
        [SerializeField, MultiLineProperty]
        private string _description;
        [SerializeField]
        private bool _isHidden = false;
        [SerializeField]
        private IQuestDefinition _nextQuest;
        [SerializeField]
        private IRewardable _questReward;

        public Guid GetID() => _questID;
        public string GetName() => _name;
        public string GetDescription() => _description;
        public bool IsHidden() => _isHidden;
        public IQuestDefinition GetNextQuest() => _nextQuest;
        public IRewardable GetReward() => _questReward;

        public abstract IQuestFactory GetQuestFactory();
    }
}
