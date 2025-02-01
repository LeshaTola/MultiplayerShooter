using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Effectors
{
    public class EffectorSpawnPoint:MonoBehaviourPun
    {
        [SerializeField] private Effector _effector;
        [SerializeField] private float _respawnTime;
        
        private CancellationTokenSource _cts;
        
        public void Respawn()
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            RespawnAsync(_cts.Token).Forget();
        }

        private async UniTaskVoid RespawnAsync(CancellationToken token)
        {
            PRCSetVisible(false);
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_respawnTime), cancellationToken: token);
                if (!token.IsCancellationRequested)
                {
                    PRCSetVisible(true);
                }
            }
            catch (OperationCanceledException) { }
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

        private void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}