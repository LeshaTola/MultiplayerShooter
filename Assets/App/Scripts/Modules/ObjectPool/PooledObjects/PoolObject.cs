using System;
using UnityEngine;

namespace App.Scripts.Modules.ObjectPool.PooledObjects
{
    [Serializable]
    public struct PoolObject<T> where T : MonoBehaviour
    {
        public int PreloadCount;
        [SerializeField] public T Template;
    }
}