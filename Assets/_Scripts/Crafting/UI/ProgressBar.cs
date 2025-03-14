using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SmelterGame.Crafting.UI
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _processName;
        [SerializeField]
        private Slider _progressSlider;

        private Func<float> _progressDelegate;

        private void Update()
        {
            _progressSlider.value = _progressDelegate?.Invoke() ?? 0f;
        }

        public void StartProgressBar(string processName, Func<float> progressDelegate)
        {
            _processName.text = processName;
            _progressDelegate = progressDelegate;
            _progressSlider.value = 0f;
            gameObject.SetActive(true);
        }

        public void StopProgressBar()
        {
            gameObject.SetActive(false);
            _progressDelegate = null;
            _progressSlider.value = 0f;
            _processName.text = "";
        }
    }
}
