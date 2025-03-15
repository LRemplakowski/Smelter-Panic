using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmelterGame.Utility
{
    public static class CollectionExtensions
    {
        public static T GetRandom<T>(this IList<T> collection)
        {
            return collection[Random.Range(0, collection.Count)];
        }
    }
}
