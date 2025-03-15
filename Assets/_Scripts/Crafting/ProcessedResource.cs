using SmelterGame.Inventory;
using UnityEngine;

namespace SmelterGame.Crafting
{
    [CreateAssetMenu(fileName = "New Processed Resource", menuName = "Items/Crafting/Processed Resource")]
    public class ProcessedResource : AbstractItem, ICraftable, IProcessable
    {

    }
}