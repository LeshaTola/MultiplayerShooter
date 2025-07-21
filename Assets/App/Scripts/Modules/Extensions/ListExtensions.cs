using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.Modules.Extensions
{
    public static class ListExtensions
    {
        public static T GetRandom<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }
    }
}