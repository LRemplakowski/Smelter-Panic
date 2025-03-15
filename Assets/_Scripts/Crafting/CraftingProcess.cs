using System;
using SmelterGame.Bonuses;
using UnityEngine;

namespace SmelterGame.Crafting
{
    public class CraftingProcess
    {
        private readonly Guid _processorID;
        private readonly float _duration;
        private readonly float _successChance;
        private readonly CraftingYield _result;
        private readonly IBonusProvider _bonusProvider;
        
        private float _startTime;
        private float _modifiedDuration;
        private bool _craftStarted;

        public CraftingProcess(Guid processorID, float duration, float successChance, CraftingYield result, IBonusProvider bonusProvider)
        {
            _processorID = processorID;
            _duration = duration;
            _successChance = successChance;
            _result = result;
            _bonusProvider = bonusProvider;
            _startTime = float.MinValue;
            _craftStarted = false;
        }

        public float GetProgress()
        {
            if (!_craftStarted)
            {
                return 0f;
            }
            if (_modifiedDuration <= 0)
            {
                return 1f;
            }
            else
            {
                float timeElapsed = Time.time - _startTime;
                float timePercentage = timeElapsed / _modifiedDuration;
                return timePercentage;
            }
        }

        public void Begin(CraftingCompletedCallback onFinishedCallback = null)
        {
            _ = CraftRecipeAsync(onFinishedCallback);
        }

        private async Awaitable CraftRecipeAsync(CraftingCompletedCallback onFinishedCallback)
        {
            _modifiedDuration = GetModifiedCraftingTime(_bonusProvider, in _duration);
            _startTime = Time.time;
            Debug.Log($"{nameof(CraftingProcess)} >>> Started crafting {_result.YieldItem.GetName()}! Time: {_modifiedDuration} seconds");
            float successRoll = UnityEngine.Random.Range(0f, 1f);
            float modifiedSuccessChance = GetModifiedSuccessChance(_bonusProvider, in _successChance);
            bool success = successRoll <= modifiedSuccessChance;
            _craftStarted = true;
            await Awaitable.WaitForSecondsAsync(_modifiedDuration);
            Debug.Log($"{nameof(CraftingProcess)} >>> Crafting success: {success}! Success chance: {modifiedSuccessChance:F3}; Success roll: {successRoll:F3}");
            onFinishedCallback?.Invoke(in _processorID, success, _result);
            Debug.Log($"{nameof(CraftingProcess)} >>> Finished processing! Yield: {_result.YieldAmount} {_result.YieldItem.GetName()}");
        }

        private static float GetModifiedSuccessChance(IBonusProvider bonusProvider, in float successChance)
        {
            if (bonusProvider.TryGetBonus(BonusCategory.SuccessRate, out var bonus))
            {
                return successChance + bonus.GetBonusValue();
            }
            return successChance;
        }

        private static float GetModifiedCraftingTime(IBonusProvider bonusProvider, in float craftingTime)
        {
            if (bonusProvider.TryGetBonus(BonusCategory.CraftingTime, out var bonus))
            {
                return craftingTime + bonus.GetBonusValue();
            }
            return craftingTime;
        }
    }
}