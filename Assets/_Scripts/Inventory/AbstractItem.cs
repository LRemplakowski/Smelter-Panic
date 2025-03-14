using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SmelterGame.Inventory
{
    public abstract class AbstractItem : SerializedScriptableObject, IItem
    {
        [SerializeField]
        private Guid _guid = Guid.NewGuid();

        [SerializeField]
        private string _name;
        [SerializeField]
        private Sprite _icon;
        [SerializeField, MultiLineProperty]
        private string _description;

        public Guid GetID() => _guid;
        public string GetName() => _name;
        public Sprite GetIcon() => _icon;
        public string GetDescription() => _description;
    }
}