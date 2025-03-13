using SmelterGame.Inventory;
using UnityEngine;

namespace SmelterGame.Crafting
{
    public delegate void CraftingCallback(ICraftable result);

    public struct CraftingProcess
    {
        private readonly float _duration;
        private readonly ICraftable _result;

        private float _startTime;
        private float _endTime;

        public CraftingProcess(float duration, ICraftable result)
        {        
            _startTime = float.MinValue;
            _endTime = float.MaxValue;
            _duration = duration;
            _result = result;
        }

        public readonly float GetProgress()
        {
            if (_duration <= 0)
            {
                Debug.LogError("");
                return 1f;
            }
            else
            {
                return (_endTime - _startTime) / _duration;
            }
        }

        public void Begin(CraftingCallback onFinishedCallback = null)
        {
            _startTime = Time.time;
            _endTime = _startTime + _duration;
            _ = CraftRecipeAsync(onFinishedCallback);
        }

        private async readonly Awaitable CraftRecipeAsync(CraftingCallback onFinishedCallback)
        {
            await Awaitable.WaitForSecondsAsync(_duration);
            onFinishedCallback?.Invoke(_result);
        }
    }
}