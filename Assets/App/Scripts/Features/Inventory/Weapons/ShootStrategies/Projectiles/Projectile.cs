using System;
using System.Threading;
using App.Scripts.Scenes.Gameplay.Player;
using App.Scripts.Scenes.Gameplay.Player.Stats;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootStrategies.Projectiles
{
    public class Projectile : MonoBehaviourPun
    {
        [SerializeField] private Health _health;
        [SerializeField] private int _maxColisionCount = 0;
        [SerializeField] private float _lifeTime = 5f;
        [SerializeField] private Rigidbody _rb;

        private Action<Vector3, GameObject> _onColisionAction;
        private CancellationTokenSource _cancellationTokenSource;
        private int _colisionCount;

        public void Setup(Action<Vector3, GameObject> onColisionAction)
        {
            _colisionCount = _maxColisionCount;
            _onColisionAction = onColisionAction;
            if (_health)
            {
                _health.OnDied += OnDied;
            }
        }

        public async void Shoot(Vector3 dir, float speed)
        {
            photonView.RPC(nameof(PushProjectile), RpcTarget.All, dir * speed);
            
            if (_lifeTime >= 0)
            {
                _cancellationTokenSource = new CancellationTokenSource();
                try
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(_lifeTime),
                        cancellationToken: _cancellationTokenSource.Token);
                    Kill();
                }
                catch (OperationCanceledException) { }
            }
        }

        [PunRPC]
        public void PushProjectile(Vector3 force)
        {
            _rb.AddForce(force, ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!photonView.IsMine)
            {
                return;
            }

            if (_colisionCount == 0)
            {
                _cancellationTokenSource?.Cancel();
                Kill(other.contacts[0].point, other.gameObject);
            }
            else
            {
                _colisionCount--;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!photonView.IsMine )
            {
                return;
            }

            if (!other.TryGetComponent(out Player player) || player.photonView.IsMine)
            {
                return;
            }
            _cancellationTokenSource?.Cancel();
            Kill(other.transform.position, other.gameObject);
        }

        private void OnDied()
        {
            Kill();
        }

        private void Kill()
        {
            Kill(transform.position, gameObject);
        }

        private void Kill(Vector3 point, GameObject hitObject)
        {
            if (_health)
            {
                _health.SetImmortal(true);
                _health.OnDied -= OnDied;
            }
            
            _onColisionAction?.Invoke(point, hitObject);
            _cancellationTokenSource?.Dispose();
            PhotonNetwork.Destroy(gameObject);
        }
    }
}