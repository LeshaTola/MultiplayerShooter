using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.Input;
using App.Scripts.Features.Inventory;
using App.Scripts.Features.Inventory.Configs;
using App.Scripts.Scenes.Gameplay.Controller;
using App.Scripts.Scenes.Gameplay.Weapons.Factories;
using Photon.Pun;
using UnityEngine;

namespace App.Scripts.Scenes.Gameplay.Weapons
{
    public class WeaponProvider : MonoBehaviourPun
    {
        public event Action<Weapon> OnWeaponChanged;
        public event Action<Vector3, float, bool> OnPlayerHit;
        
        [SerializeField] private Transform _weaponHolder;
        [SerializeField] private GlobalInventory _globalInventory;
        
        public List<Weapon> Weapons { get; } = new();
        private GameInputProvider _gameInputProvider;
        private InventoryProvider _inventoryProvider;
        private ShootingModeFactory _shootingModeFactory;
        private PlayerController _playerController;

        private int _weaponIndex;
        public Weapon CurrentWeapon { get; private set; }

        public void OnDestroy()
        {
            Cleanup();
        }

        public void Initialize(GameInputProvider gameInputProvider,
            PlayerController playerController,
            InventoryProvider inventoryProvider, 
            ShootingModeFactory shootingModeFactory,
            Player.Player owner)
        {
            _gameInputProvider = gameInputProvider;
            _inventoryProvider = inventoryProvider;
            _shootingModeFactory = shootingModeFactory;
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
                weaponObject.Initialize(newConfig);
                
                weaponObject.OnPlayerHit += (value, damage, killed) =>
                {
                    OnPlayerHit?.Invoke(value, damage, killed);
                    if (killed)
                    {
                        owner.PlayerAudioProvider.PlayKillSound();
                    }
                    else
                    {
                        owner.PlayerAudioProvider.PlayHitSound();
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
            var index = Weapons.FindIndex(x=>x);
            RPCSetWeaponByIndex(index + 1);
        }

        private void OnScrollWheel(float scroll)
        {
            _weaponIndex = scroll > 0 ? _weaponIndex - 1 : _weaponIndex + 1;
            if (_weaponIndex == -1)
            {
                _weaponIndex = Weapons.Count - 1;
            }

            if (_weaponIndex == Weapons.Count)
            {
                _weaponIndex = 0;
            }
            
            RPCSetWeaponByIndex(_weaponIndex + 1);
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
            _weaponIndex = index;
            
            var weapon = Weapons[index];
            if (!weapon || weapon == CurrentWeapon)
            {
                return;
            }
            
            OnWeaponChanged?.Invoke(weapon);
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
            
            weapon.SetupPlayer(player);
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