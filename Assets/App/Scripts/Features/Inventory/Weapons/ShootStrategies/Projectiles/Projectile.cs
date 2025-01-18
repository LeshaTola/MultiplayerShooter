using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootStrategies.Projectiles
{
    public class Projectile : MonoBehaviourPun
    {
        [SerializeField] private float _lifeTime = 5f;
        [SerializeField] private Rigidbody _rb;

        private Action<Vector3, GameObject> _onColisionAction;
        private CancellationTokenSource _cancellationTokenSource;

        public void Setup(Action<Vector3, GameObject> onColisionAction)
        {
            _onColisionAction = onColisionAction;
        }

        public async void Shoot(Vector3 dir, float speed)
        {
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                _rb.AddForce(dir * speed, ForceMode.Impulse);
                await UniTask.Delay(TimeSpan.FromSeconds(_lifeTime), cancellationToken: _cancellationTokenSource.Token);
                
                _onColisionAction?.Invoke(transform.position, gameObject);
                _cancellationTokenSource?.Dispose();
                PhotonNetwork.Destroy(gameObject);
            }
            catch (OperationCanceledException) { }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!photonView.IsMine)
            {
                return;
            }
            
            _onColisionAction?.Invoke(other.contacts[0].point, other.gameObject);
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            PhotonNetwork.Destroy(gameObject);
        }
    }
}