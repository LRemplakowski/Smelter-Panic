using System;

namespace SmelterGame.Bonuses
{
    public interface IBonus : IComparable<IBonus>
    {
        Guid GetSourceID();
        string GetName();
        string GetDescription();
        BonusCategory GetBonusCategory();
        float GetBonusValue();
    }
}
