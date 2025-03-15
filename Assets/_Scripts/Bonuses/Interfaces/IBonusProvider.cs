using System.Collections.Generic;

namespace SmelterGame.Bonuses
{
    public interface IBonusProvider
    {
        bool TryGetBonus(BonusCategory bonusCategory, out IBonus bonus);
        IReadOnlyCollection<IBonus> GetAllActiveBonuses();
    }
}
