using System;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Effectors
{
    public class EffectorSpawnPoint:MonoBehaviourPun
    {
        [SerializeField] private Effector _effector;
        [SerializeField] private float _respawnTime;
        
        public async void Respawn()
        {
            PRCSetVisible(false);
            await UniTask.Delay(TimeSpan.FromSeconds(_respawnTime));
            PRCSetVisible(true);
        }
        
        public void PRCSetVisible(bool active)
        {
            photonView.RPC(nameof(SetVisible), RpcTarget.All, active);
        }

        [PunRPC]
        public void SetVisible(bool active)
        {
            _effector.gameObject.SetActive(active);
        }
    }
}