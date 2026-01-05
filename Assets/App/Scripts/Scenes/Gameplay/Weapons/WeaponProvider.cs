using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.Input;
using App.Scripts.Features.Inventory;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Modules.ObjectPool.KeyPools;
using App.Scripts.Modules.ObjectPool.PooledObjects;
using App.Scripts.Scenes.Gameplay.Controller;
using App.Scripts.Scenes.Gameplay.Weapons.Factories;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Weapons
{
    public class WeaponProvider : WeaponProviderBase
    {
        public override event Action<Weapon> OnWeaponChanged;
        public override event Action<int> OnWeaponIndexChanged;
        public event Action<Vector3, float, bool> OnPlayerHit;
        
        [SerializeField] private Transform _weaponHolder;
        [SerializeField] private GlobalInventory _globalInventory;

        private IGameInputProvider _gameInputProvider;
        private InventoryProvider _inventoryProvider;
        private ShootingModeFactory _shootingModeFactory;
        private PlayerController _playerController;
        private TargetDetector.TargetDetector _detector;

        public void OnDestroy()
        {
            Cleanup();
        }

        public void Initialize(IGameInputProvider gameInputProvider,
            PlayerController playerController,
            InventoryProvider inventoryProvider, 
            ShootingModeFactory shootingModeFactory,
            TargetDetector.TargetDetector detector,
            KeyPool<PoolableParticle> particlesPool,
            Player.Player owner,
            Camera cameraMain)
        {
            _gameInputProvider = gameInputProvider;
            _inventoryProvider = inventoryProvider;
            _shootingModeFactory = shootingModeFactory;
            _detector = detector;
            _playerController = playerController;
            
            foreach (var weaponId in _inventoryProvider.GameInventory.Weapons)
            {
                if (string.IsNullOrEmpty(weaponId))
                {
                    photonView.RPC(nameof(SetupWeapon), RpcTarget.AllBuffered, -1, -1, "");
                    continue;
                }

                var weaponConfig = inventoryProvider.WeaponById(weaponId);
                var weaponObject
                    = PhotonNetwork.Instantiate(weaponConfig.Prefab.name, _weaponHolder.position, _weaponHolder.rotation)
                        .GetComponent<Weapon>();
                
                var newConfig = GetNewConfig(weaponConfig);
                weaponObject.Initialize(newConfig, _detector,particlesPool);
                weaponObject.CustomOrigin = cameraMain.transform.position;
                
                weaponObject.OnPlayerHit += (value, damage, killed) =>
                {
                    OnPlayerHit?.Invoke(value, damage, killed);
                    if (killed)
                    {
                        owner.AudioProvider.PlayKillSound();
                    }
                    else
                    {
                        owner.AudioProvider.PlayHitSound();
                    }
                };
                photonView.RPC(nameof(SetupWeapon),
                    RpcTarget.AllBuffered, 
                    weaponObject.GetComponent<PhotonView>().ViewID, 
                    owner.GetComponent<PhotonView>().ViewID,
                    weaponId);
            }
            
            _gameInputProvider.OnNumber += RPCSetWeaponByIndex;
            _gameInputProvider.OnScrollWheel += OnScrollWheel;
            _gameInputProvider.OnAutoChanged += SetAutoOnWeapon;
            var index = Weapons.FindIndex(x=>x);
            RPCSetWeaponByIndex(index + 1);
        }

        private void SetAutoOnWeapon(bool isAuto)
        {
            CurrentWeapon.IsAutoShoot = isAuto;
        }

        private void OnScrollWheel(float scroll)
        {
            WeaponIndex = scroll > 0 ? WeaponIndex - 1 : WeaponIndex + 1;
            if (WeaponIndex == -1)
            {
                WeaponIndex = Weapons.Count - 1;
            }

            if (WeaponIndex == Weapons.Count)
            {
                WeaponIndex = 0;
            }
            
            RPCSetWeaponByIndex(WeaponIndex + 1);
        }

        public void Cleanup()
        {
            if (_gameInputProvider == null)
            {
                return;
            }
            
            _gameInputProvider.OnNumber -= RPCSetWeaponByIndex;
            _gameInputProvider.OnScrollWheel -= OnScrollWheel;
        }

        private void RPCSetWeaponByIndex(int index)
        {
            if (_playerController.IsBusy)
            {
                return;
            }
            
            index--;
            index = Math.Clamp(index, 0, Weapons.Count - 1);
            WeaponIndex = index;
            
            var weapon = Weapons[index];
            if (!weapon || weapon == CurrentWeapon)
            {
                return;
            }
            
            OnWeaponChanged?.Invoke(weapon);
            _gameInputProvider.SetAuto(weapon.IsAutoShoot);
            OnWeaponIndexChanged?.Invoke(index);
            if (CurrentWeapon != null)
            {
                CurrentWeapon.GuaranteedCancelAttack(true);
                CurrentWeapon.GuaranteedCancelAttack(false);
            }
            photonView.RPC(nameof(SetWeaponByIndex), RpcTarget.AllBuffered, index);
        }

        [PunRPC]
        public void SetupWeapon(int weaponViewID,int ownerId, string id)
        {
            if (weaponViewID == -1)
            {
                Weapons.Add(null);
                return;
            }
            
            var weaponObject = PhotonView.Find(weaponViewID).gameObject;
            var player = PhotonView.Find(ownerId).GetComponent<Player.Player>();
            var weapon = weaponObject.GetComponent<Weapon>();
            Weapons.Add(weapon);
            
            weapon.SetupOwner(player);
            if(!photonView.IsMine)
                weapon.SetupLocalConfig(_globalInventory.Weapons.FirstOrDefault(x => x.Id == id));

            var weaponTransform = weapon.transform;
            weaponTransform.SetParent(_weaponHolder);
            weapon.gameObject.SetActive(false);
        }

        [PunRPC]
        public void SetWeaponByIndex(int index)
        {
            if (CurrentWeapon != null)
            {
                CurrentWeapon.gameObject.SetActive(false);
            }

            CurrentWeapon = Weapons[index];
            CurrentWeapon.gameObject.SetActive(true);
        }

        private WeaponConfig GetNewConfig(WeaponConfig weaponConfig)
        {
            var newConfig = Instantiate(weaponConfig);
            var shootingMode = _shootingModeFactory.GetShootingMode(weaponConfig.ShootingMode);
            var shootingModeAlternative = _shootingModeFactory.GetShootingMode(weaponConfig.ShootingModeAlternative);
            newConfig.Initialize(shootingMode,shootingModeAlternative);
            return newConfig;
        }
    }
}