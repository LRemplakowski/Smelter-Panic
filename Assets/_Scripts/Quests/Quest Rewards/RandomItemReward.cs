using System.Collections.Generic;
using SmelterGame.Inventory;
using SmelterGame.Utility;
using UnityEngine;

namespace SmelterGame.Quests
{
    public class RandomItemReward : AbstractItemReward
    {
        [SerializeField]
        private List<IItem> _possibleItems = new();

        public override IItem GetRewardedItem()
        {
            return _possibleItems.GetRandom();
        }
    }
}