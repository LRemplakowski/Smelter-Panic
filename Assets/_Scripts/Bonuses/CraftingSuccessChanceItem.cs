using Sirenix.OdinInspector;
using UnityEngine;

namespace SmelterGame.Bonuses
{
    [CreateAssetMenu(fileName = "New Crafting Time Reduction Item", menuName = "Items/Bonuses/Crafting Success Chance")]
    public class CraftingSuccessChanceItem : BonusDefinitionItem
    {
        [SerializeField, PropertyRange(0f, 2f), InfoBox("Increase to crafting success chance. Value of 1 grants +100% success chance.")]
        private float _successChanceBonus = .1f;

        private IBonusFactory _bonusFactory;

        public override BonusCategory GetBonusCategory() => BonusCategory.SuccessRate;
        public override float GetBonusValue() => _successChanceBonus;

        public override IBonusFactory GetBonusFactory()
        {
            EnsureBonusFactory();
            return _bonusFactory;
        }

        private void EnsureBonusFactory()
        {
            _bonusFactory ??= new CraftingSucccessChanceBonusFactory(this);
        }

        private class CraftingSucccessChanceBonusFactory : IBonusFactory
        {
            private readonly CraftingSuccessChanceItem _bonusDefinition;

            public CraftingSucccessChanceBonusFactory(CraftingSuccessChanceItem bonusDefinition)
            {
                _bonusDefinition = bonusDefinition;
            }

            public IBonus Create()
            {
                return new CraftingSuccessChanceBonus(_bonusDefinition);
            }
        }
    }
}