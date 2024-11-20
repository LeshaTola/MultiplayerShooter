using System.Collections.Generic;
using App.Scripts.Modules.ObjectPool.Pools;
using UnityEngine;

namespace App.Scripts.Modules.ObjectPool.MonoObjectPools
{
    public class MonoBehObjectPool<T> : IPool<T> where T : MonoBehaviour
    {
        private ObjectPool<T> core;

        public IReadOnlyCollection<T> Active => core.Active;

        public MonoBehObjectPool(T objectTemplate, int startCount, Transform parent = null)
        {
            core = new ObjectPool<T>(
                () =>
                {
                    var pooledObject = GameObject.Instantiate(objectTemplate, parent);
                    pooledObject.gameObject.SetActive(false);
                    return pooledObject;
                },
                (obj) => { obj.gameObject.SetActive(true); },
                (obj) =>
                {
                    obj.gameObject.SetActive(false);
                    if (parent != null)
                    {
                        obj.transform.SetParent(parent);
                    }
                },
                startCount
            );
        }

        public T Get()
        {
            var pooledObject = core.Get();
            return pooledObject;
        }

        public void Release(T pooledObject)
        {
            core.Release(pooledObject);
        }
    }
}