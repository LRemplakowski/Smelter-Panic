using System;
using System.Collections.Generic;
using UnityEngine;

namespace SmelterGame.Quests.UI
{
    public class QuestUI : MonoBehaviour
    {
        [SerializeField]
        private QuestView _questViewPrefab;
        [SerializeField]
        private Transform _questViewParent;

        private readonly Dictionary<Guid, QuestView> _trackedQuests = new();

        private IQuestViewFactory _questViewFactory;

        private void Awake()
        {
            EnsureQuestViewFactory();
            QuestManager.OnQuestStarted += OnQuestStarted;
            QuestManager.OnQuestUpdated += OnQuestUpdated;
            QuestManager.OnQuestCompleted += OnQuestCompleted;
        }

        private void OnDestroy()
        {
            QuestManager.OnQuestStarted -= OnQuestStarted;
            QuestManager.OnQuestUpdated -= OnQuestUpdated;
            QuestManager.OnQuestCompleted -= OnQuestCompleted;
        }

        private void OnQuestStarted(IQuest quest)
        {
            if (quest.IsHidden())
            {
                // noop
                // Do Nothing, Quest shouldn't be visible to player
                return;
            }
            else
            {
                var questView = GetViewFactory().Create(quest);
                GetTrackedQuests()[quest.GetID()] = questView;
            }
        }

        private void OnQuestUpdated(IQuest quest)
        {
            if (GetTrackedQuests().TryGetValue(quest.GetID(), out var questView))
            {
                questView.UpdateView(quest);
            }
        }

        private void OnQuestCompleted(IQuest quest)
        {
            if (GetTrackedQuests().TryGetValue(quest.GetID(), out var questView))
            {
                Destroy(questView.gameObject);
                GetTrackedQuests().Remove(quest.GetID());
            }
        }

        private void EnsureQuestViewFactory()
        {
            _questViewFactory ??= new DefaultQuestViewFactory(GetQuestViewPrefab(), GetQuestViewParent());
        }

        private Dictionary<Guid, QuestView> GetTrackedQuests() => _trackedQuests;
        private IQuestViewFactory GetViewFactory() => _questViewFactory;
        private QuestView GetQuestViewPrefab() => _questViewPrefab;
        private Transform GetQuestViewParent() => _questViewParent;

        private interface IQuestViewFactory
        {
            QuestView Create(IQuest quest);
        }

        private class DefaultQuestViewFactory : IQuestViewFactory
        {
            private readonly QuestView _viewPrefab;
            private readonly Transform _viewParent;

            public DefaultQuestViewFactory(QuestView viewPrefab, Transform viewParent)
            {
                _viewPrefab = viewPrefab;
                _viewParent = viewParent;
            }

            public QuestView Create(IQuest quest)
            {
                var questView = Instantiate(_viewPrefab, _viewParent);
                questView.Initialize(quest);
                return questView;
            }
        }
    }
}
