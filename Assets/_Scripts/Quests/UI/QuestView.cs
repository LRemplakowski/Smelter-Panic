using TMPro;
using UnityEngine;

namespace SmelterGame.Quests.UI
{
    public class QuestView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _questTitle;
        [SerializeField]
        private TextMeshProUGUI _questProgress;

        public void Initialize(IQuest quest)
        {
            _questTitle.text = quest.GetName();
            _questProgress.text = quest.GetProgressText();
        }

        public void UpdateView(IQuest quest)
        {
            _questProgress.text = quest.GetProgressText();
        }
    }
}