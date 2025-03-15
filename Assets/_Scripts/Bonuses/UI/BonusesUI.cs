using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace SmelterGame.Bonuses.UI
{
    public class BonusesUI : MonoBehaviour
    {
        [SerializeField, Required]
        private IBonusProvider _bonusProvider;

        [Title("Active Bonuses")]
        [SerializeField]
        private BonusView _bonusViewPrefab;
        [SerializeField]
        private Transform _bonusViewParent;

        private IBonusViewFactory _bonusViewFactory;
        private readonly List<BonusView> _activeBonusViews = new();

        private void Awake()
        {
            EnsureBonusViewFactory();
            BonusManager.OnActiveBonusesUpdated += OnActiveBonusesUpdated;
        }

        private void OnDestroy()
        {
            BonusManager.OnActiveBonusesUpdated -= OnActiveBonusesUpdated;
        }

        private void OnActiveBonusesUpdated(IReadOnlyCollection<IBonus> activeBonuses)
        {
            CleanupBonusViews();
            CreateBonusViews(activeBonuses);
        }

        private void CreateBonusViews(IEnumerable<IBonus> bonuses)
        {
            foreach (var bonus in bonuses)
            {
                var view = GetBonusViewFactory().Create(bonus);
                GetActiveBonusViews().Add(view);
            }
        }

        private void CleanupBonusViews()
        {
            GetActiveBonusViews().ForEach(view => Destroy(view.gameObject));
            GetActiveBonusViews().Clear();
        }

        private void EnsureBonusViewFactory()
        {
            _bonusViewFactory ??= new DefaultBonusViewFactory(GetBonusViewPrefab(), GetBonusViewParent());
        }

        private BonusView GetBonusViewPrefab() => _bonusViewPrefab;
        private Transform GetBonusViewParent() => _bonusViewParent;
        private IBonusViewFactory GetBonusViewFactory() => _bonusViewFactory;
        private List<BonusView> GetActiveBonusViews() => _activeBonusViews;

        private interface IBonusViewFactory
        {
            BonusView Create(IBonus bonus);
        }

        private class DefaultBonusViewFactory : IBonusViewFactory
        {
            private readonly BonusView _viewPrefab;
            private readonly Transform _viewParent;

            public DefaultBonusViewFactory(BonusView viewPrefab, Transform viewParent)
            {
                _viewPrefab = viewPrefab;
                _viewParent = viewParent;
            }

            public BonusView Create(IBonus bonus)
            {
                var bonusView = Instantiate(_viewPrefab, _viewParent);
                bonusView.Initialize(bonus);
                return bonusView;
            }
        }
    }
}
