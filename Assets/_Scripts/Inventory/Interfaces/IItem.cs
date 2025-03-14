using System;
using UnityEngine;

namespace SmelterGame.Inventory
{
    public interface IItem
    {
        Guid GetID();
        Sprite GetIcon();
        string GetName();
        string GetDescription();
    }
}
