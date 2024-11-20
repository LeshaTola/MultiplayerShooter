using System;
using UnityEngine;

namespace App.Scripts.Modules.Saves.Structs
{
    [Serializable]
    public struct JsonVector2Int
    {
        public int X, Y;

        public JsonVector2Int(Vector2Int vector)
        {
            X = vector.x;
            Y = vector.y;
        }
    }
}