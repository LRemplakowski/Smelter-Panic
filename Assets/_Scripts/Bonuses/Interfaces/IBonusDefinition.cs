using System;

namespace SmelterGame.Bonuses
{
    public interface IBonusDefinition
    {
        Guid GetID();
        string GetName();
        string GetDescription();
        BonusCategory GetBonusCategory();
        float GetBonusValue();
        IBonusFactory GetBonusFactory();
    }

    public interface IBonusFactory
    {
        IBonus Create();
    }
}
