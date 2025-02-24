using System.Collections.Generic;
using System.Threading;
using App.Scripts.Features.Inventory.Weapons.ShootStrategies.Laser;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Features.Inventory.Weapons.ShootStrategies
{
    public class LaserShootStrategy : ShootStrategy
    {
        [SerializeField] private float _maxDistance = 50f;
        [SerializeField] private float _cooldown = 0.2f;
        [SerializeField] private LaserRenderer _laserTemplate;

        private readonly Camera _camera;

        private CancellationTokenSource _cancellationTokenSource;
        private bool _isShooting;
        private float _timer;
        private LaserRenderer _laser;
        
        private List<(Vector3, GameObject)> _data = new();

        public LaserShootStrategy(Camera camera)
        {
            _camera = camera;
        }

        public override void Shoot()
        {
            if (_isShooting)
            {
                StopLaser();
            }
            else
            {
                StartLaser();
            }
        }

        private void StartLaser()
        {
            if (!_laser)
            {
                _laser  = Object.Instantiate(_laserTemplate, Weapon.transform);
                _laser.transform.position = Weapon.NextShootPoint(); 
                _laser.Initialize(Weapon.ShootPointProvider.ShotPoint, GetRaycastHit().Item2);
            }
            else
            {
                _laser.ShowRPC();
            }
            
            _isShooting = true;

            _cancellationTokenSource = new CancellationTokenSource();
            DamageOverTimeAsync(_cancellationTokenSource.Token).Forget();
        }

        private void StopLaser()
        {
            _isShooting = false;
            _laser.HideRPC();

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }
        
        private async UniTaskVoid DamageOverTimeAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            { 
                await UniTask.Yield();
                _laser.SetLength(GetRaycastHit().Item2);
                _timer -= Time.deltaTime;
                if (_timer <= 0)
                {
                    ShootInternal();
                    _timer = _cooldown;
                }
            }
        }

        public override void Import(IShootStrategy original)
        {
            base.Import(original);

            var concrete = (LaserShootStrategy) original;
            _maxDistance = concrete._maxDistance;
            _cooldown = concrete._cooldown;
            _laserTemplate = concrete._laserTemplate;
        }


        private void ShootInternal()
        {
            Recoil.Add();
            var hitData = GetRaycastHit();
            
            _laser.SetLengthRPC(GetRaycastHit().Item2);
            if (!hitData.Item1.collider)
            {
                return;
            }

            _data.Clear();
            _data.Add((hitData.Item1.point, hitData.Item1.collider.gameObject));

            foreach (var weaponEffect in WeaponEffects)
            {
                weaponEffect.Effect(_data);
            }
        }

        private (RaycastHit, Vector3) GetRaycastHit()
        {
            var recoilRotation = Recoil.GetRecoilRotation(_camera.transform);
            var direction = recoilRotation * _camera.transform.forward;

            Vector3 endPoint = _camera.transform.position + direction * _maxDistance;
            if (Physics.Raycast(_camera.transform.position, direction, out var hit, _maxDistance, Physics.DefaultRaycastLayers,
                    QueryTriggerInteraction.Ignore))
            {
                endPoint = hit.point;
            }

            return (hit, endPoint);
        }
    }
}