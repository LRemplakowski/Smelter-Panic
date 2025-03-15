namespace SmelterGame.Bonuses
{
    public interface IBonusProvider
    {
        bool TryGetBonus(BonusCategory bonusCategory, out IBonus bonus);
    }
}
