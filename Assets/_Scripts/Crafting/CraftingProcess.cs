using System;
using UnityEngine;

namespace SmelterGame.Crafting
{
    public struct CraftingProcess
    {
        private readonly Guid _processorID;
        private readonly float _duration;
        private readonly double _successChance;
        private readonly CraftingYield _result;

        private float _startTime;
        private float _endTime;

        public CraftingProcess(Guid processorID, float duration, double successChance, CraftingYield result)
        {
            _processorID = processorID;
            _duration = duration;
            _successChance = successChance;
            _result = result;
            _startTime = float.MinValue;
            _endTime = float.MaxValue;
        }

        public readonly float GetProgress()
        {
            if (_duration <= 0)
            {
                Debug.LogError($"{nameof(CraftingProcess)} >>> Crafting of {_result} has invalid duration!");
                return 1f;
            }
            else
            {
                return (_endTime - _startTime) / _duration;
            }
        }

        public void Begin(CraftingCompletedCallback onFinishedCallback = null)
        {
            Debug.Log($"{nameof(CraftingProcess)} >>> Started processing to {_result.YieldItem.GetName()}! Time: {_duration}");
            _startTime = Time.time;
            _endTime = _startTime + _duration;
            _ = CraftRecipeAsync(onFinishedCallback);
        }

        private async readonly Awaitable CraftRecipeAsync(CraftingCompletedCallback onFinishedCallback)
        {
            bool success = UnityEngine.Random.Range(0f, 1f) <= _successChance;
            await Awaitable.WaitForSecondsAsync(_duration);
            onFinishedCallback?.Invoke(_processorID, success, _result);
            Debug.Log($"{nameof(CraftingProcess)} >>> Finished processing! Yield: {_result.YieldAmount} {_result.YieldItem.GetName()}");
        }
    }
}