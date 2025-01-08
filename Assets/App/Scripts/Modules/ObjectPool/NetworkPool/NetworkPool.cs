using System.Collections.Generic;
using App.Scripts.Modules.ObjectPool.Pools;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Modules.ObjectPool.NetworkPool
{
    public class NetworkPool<T>: IPool<T> where T : MonoBehaviour
    {
        private ObjectPool<T> core;

        public IReadOnlyCollection<T> Active => core.Active;
        
        public NetworkPool(T objectTemplate, int startCount, Transform parent)
        {
            core = new ObjectPool<T>(
                () =>
                {
                    var pooledObject = PhotonNetwork.Instantiate(objectTemplate.name, parent.position,parent.rotation);
                    pooledObject.transform.parent = parent;
                    pooledObject.gameObject.SetActive(false);
                    return pooledObject.GetComponent<T>();
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