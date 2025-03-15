using System;
using UnityEngine;

namespace SmelterGame.Crafting
{
    public class CraftingProcess
    {
        private readonly Guid _processorID;
        private readonly float _duration;
        private readonly double _successChance;
        private readonly CraftingYield _result;
        
        private float _startTime;
        private bool _craftStarted;

        public CraftingProcess(Guid processorID, float duration, double successChance, CraftingYield result)
        {
            _processorID = processorID;
            _duration = duration;
            _successChance = successChance;
            _result = result;
            _startTime = float.MinValue;
            _craftStarted = false;
        }

        public float GetProgress()
        {
            if (!_craftStarted)
            {
                return 0f;
            }
            if (_duration <= 0)
            {
                Debug.LogError($"{nameof(CraftingProcess)} >>> Crafting of {_result} has invalid duration!");
                return 1f;
            }
            else
            {
                float timeElapsed = Time.time - _startTime;
                float timePercentage = timeElapsed / _duration;
                return timePercentage;
                //return (_startTime - Time.time) / _duration;
            }
        }

        public void Begin(CraftingCompletedCallback onFinishedCallback = null)
        {
            Debug.Log($"{nameof(CraftingProcess)} >>> Started processing to {_result.YieldItem.GetName()}! Time: {_duration} seconds");
            _ = CraftRecipeAsync(onFinishedCallback);
        }

        private async Awaitable CraftRecipeAsync(CraftingCompletedCallback onFinishedCallback)
        {
            float successRoll = UnityEngine.Random.Range(0f, 1f);
            bool success = successRoll <= _successChance;
            Debug.Log($"{nameof(CraftingProcess)} >>> Crafting success: {success}! Success chance: {_successChance:F3}; Success roll: {successRoll:F3}");
            _startTime = Time.time;
            _craftStarted = true;
            await Awaitable.WaitForSecondsAsync(_duration);
            onFinishedCallback?.Invoke(in _processorID, success, _result);
            Debug.Log($"{nameof(CraftingProcess)} >>> Finished processing! Yield: {_result.YieldAmount} {_result.YieldItem.GetName()}");
        }
    }
}