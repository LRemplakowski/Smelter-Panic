using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SmelterGame.Crafting.UI
{
    public class ProcessorSlotView : MonoBehaviour
    {
        [SerializeField]
        private Button _slotButton;
        [SerializeField]
        private Image _processorIcon;
        [SerializeField]
        private TextMeshProUGUI _processorName;
        [SerializeField]
        private ProgressBar _progressBar;

        private Guid _cachedID;
        private Action<Guid> _onClickDelegate;

        public void Initialize(IProcessorDefinition processorDefinition, Action<Guid> onClickDelegate)
        {
            _cachedID = processorDefinition.GetID();
            _onClickDelegate = onClickDelegate;
            _processorIcon.sprite = processorDefinition.GetIcon();
            _processorName.text = processorDefinition.GetName();
        }

        public void OnSlotClicked()
        {
            _onClickDelegate?.Invoke(_cachedID);
        }

        public ProgressBar GetProgressBar() => _progressBar;
        public void SetSlotInteractable(bool interactable) => _slotButton.interactable = interactable;
    }
}
