using SmelterGame.Crafting;
using UnityEngine;

namespace SmelterGame.Inventory
{
    [CreateAssetMenu(fileName = "New Basic Resource", menuName = "Items/Basic Resource")]
    public class BasicResource : ScriptableObject, IItem, IProcessable
    {
        [field: SerializeField]
        public string Name { get; private set; }
    }
}
