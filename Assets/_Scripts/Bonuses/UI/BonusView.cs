using TMPro;
using UnityEngine;

namespace SmelterGame.Bonuses.UI
{
    public class BonusView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _bonusName;
        [SerializeField]
        private TextMeshProUGUI _bonusDescription;

        public void Initialize(IBonus bonus)
        {
            _bonusName.text = bonus.GetName();
            _bonusDescription.text = bonus.GetDescription();
        }
    }
}
