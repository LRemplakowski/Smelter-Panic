using System;
using Sirenix.OdinInspector;

namespace SmelterGame.Bonuses
{
    public abstract class AbstractBonus : IBonus
    {
        [ShowInInspector, ReadOnly]
        private readonly IBonusDefinition _definition;

        public AbstractBonus(IBonusDefinition definition)
        {
            _definition = definition;
        }

        public Guid GetSourceID() => _definition.GetID();
        public string GetName() => _definition.GetName();
        public string GetDescription() => _definition.GetDescription();
        public BonusCategory GetBonusCategory() => _definition.GetBonusCategory();
        public float GetBonusValue() => _definition.GetBonusValue();

        public abstract int CompareTo(IBonus other);
    }
}