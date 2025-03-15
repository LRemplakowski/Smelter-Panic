using SmelterGame.Inventory;

namespace SmelterGame.Bonuses
{
    public abstract class BonusDefinitionItem : AbstractItem, IBonusDefinition
    {
        public abstract BonusCategory GetBonusCategory();
        public abstract float GetBonusValue();


        public abstract IBonusFactory GetBonusFactory();
    }
}