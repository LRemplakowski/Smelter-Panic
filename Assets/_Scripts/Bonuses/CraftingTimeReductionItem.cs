using Sirenix.OdinInspector;
using UnityEngine;

namespace SmelterGame.Bonuses
{
    [CreateAssetMenu(fileName = "New Crafting Time Reduction Item", menuName = "Items/Bonuses/Crafting Time Reduction")]
    public class CraftingTimeReductionItem : BonusDefinitionItem
    {
        [SerializeField, MaxValue(0f), InfoBox("Amount of time reduced for crafting. Lower is better.")]
        private float _timeReduction = -1f;

        private IBonusFactory _bonusFactory;

        public override BonusCategory GetBonusCategory() => BonusCategory.CraftingTime;
        public override float GetBonusValue() => _timeReduction;

        public override IBonusFactory GetBonusFactory()
        {
            EnsureBonusFactory();
            return _bonusFactory;
        }

        private void EnsureBonusFactory()
        {
            _bonusFactory ??= new CraftingTimeReductionBonusFactory(this);
        }

        private class CraftingTimeReductionBonusFactory : IBonusFactory
        {
            private readonly CraftingTimeReductionItem _bonusDefinition;

            public CraftingTimeReductionBonusFactory(CraftingTimeReductionItem bonusDefinition)
            {
                _bonusDefinition = bonusDefinition;
            }

            public IBonus Create()
            {
                return new CraftingTimeReductionBonus(_bonusDefinition);
            }
        }
    }
}