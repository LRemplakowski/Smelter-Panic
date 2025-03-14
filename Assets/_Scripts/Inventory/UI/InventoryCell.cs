using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SmelterGame.Inventory.UI
{
    public class InventoryCell : SerializedMonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IItemProvider
    {
        [Title("Config")]
        [SerializeField]
        private Image _itemIcon;
        [SerializeField]
        private TextMeshProUGUI _itemCount;
        [SerializeField]
        private Sprite _defaultSprite;
        [Title("Runtime")]
        [ShowInInspector]
        private IItem _cellItem;
        [ShowInInspector]
        private int _amount;
        [ShowInInspector]
        private Transform _defaultParent;
        [ShowInInspector]
        private Transform _dragParent;
        [ShowInInspector]
        private RectTransform _dragRect;
        private int _cachedSiblingIndex;

        public bool IsEmpty() => _cellItem == null || _amount <= 0;

        private void Start()
        {
            //_itemCount.enabled = false;
            //_itemIcon.sprite = _defaultSprite;
            _dragRect = transform as RectTransform;
        }

        public void Initialize(Transform defaultParent, Transform dragParent, IItem item, int amount)
        {
            _defaultParent = defaultParent;
            _dragParent = dragParent;
            UpdateCell(item, amount);
        }

        public void UpdateCell(IItem item, int amount)
        {
            _cellItem = item;
            _amount = amount;
            _itemCount.enabled = !IsEmpty();
            if (IsEmpty())
            {
                _itemIcon.sprite = _defaultSprite;
            }
            else
            {
                _itemIcon.sprite = item.GetIcon();
                _itemCount.text = $"{amount}";
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (IsEmpty())
            {
                return;
            }
            _cachedSiblingIndex = transform.GetSiblingIndex();
            transform.SetParent(_dragParent);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (IsEmpty())
            {
                return;
            }
            _dragRect.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (IsEmpty())
            {
                return;
            }
            transform.SetParent(_defaultParent);
            transform.SetSiblingIndex(_cachedSiblingIndex);
        }

        public IItem GetItem() => _cellItem;
    }

    public interface IItemProvider
    {
        IItem GetItem();
    }
}
