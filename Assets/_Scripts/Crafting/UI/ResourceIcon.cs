using UnityEngine;
using UnityEngine.UI;

namespace SmelterGame.Crafting.UI
{
    public class ResourceIcon : MonoBehaviour
    {
        [SerializeField]
        private Selectable _selectable;
        [SerializeField]
        private Image _icon;

        public void Initialize(Sprite icon)
        {
            _icon.sprite = icon;
        }

        public void SetActive(bool active) => _selectable.interactable = active;
    }
}
