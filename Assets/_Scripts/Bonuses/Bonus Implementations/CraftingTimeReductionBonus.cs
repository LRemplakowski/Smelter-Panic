using System;

namespace SmelterGame.Bonuses
{
    public class CraftingTimeReductionBonus : AbstractBonus
    {
        public CraftingTimeReductionBonus(IBonusDefinition bonusDefinition) : base(bonusDefinition)
        {

        }

        public override int CompareTo(IBonus other)
        {
            if (other is not CraftingTimeReductionBonus otherBonus)
            {
                throw new ArgumentException($"{nameof(CraftingTimeReductionBonus)} >>> Other is not a Crafting Time Reduction Bonus!");
            }
            // Invert comparison, lower time reduction is better
            return -GetBonusValue().CompareTo(otherBonus.GetBonusValue());
        }
    }
}