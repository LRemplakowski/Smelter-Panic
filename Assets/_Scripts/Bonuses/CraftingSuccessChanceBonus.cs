using System;

namespace SmelterGame.Bonuses
{
    public class CraftingSuccessChanceBonus : AbstractBonus
    {
        public CraftingSuccessChanceBonus(IBonusDefinition bonusDefinition) : base(bonusDefinition)
        {

        }

        public override int CompareTo(IBonus other)
        {
            if (other is not CraftingSuccessChanceBonus otherBonus)
            {
                throw new ArgumentException($"{nameof(CraftingSuccessChanceBonus)} >>> Other is not a Crafting Success Chance Bonus!");
            }
            return GetBonusValue().CompareTo(otherBonus.GetBonusValue());
        }
    }
}