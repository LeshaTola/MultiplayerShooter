using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.Input;
using App.Scripts.Features.Inventory;
using App.Scripts.Features.Inventory.Weapons;
using App.Scripts.Scenes.Gameplay.Weapons.Factories;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace App.Scripts.Scenes.Gameplay.Weapons
{
    public class WeaponProvider : MonoBehaviourPun
    {
        public event Action<Weapon> OnWeaponChanged;
        public event Action<Vector3, float> OnPlayerHit;
        
        [SerializeField] private Transform _weaponHolder;
        
        public List<Weapon> Weapons { get; } = new();
        private GameInputProvider _gameInputProvider;
        private InventoryProvider _inventoryProvider;
        private ShootingModeFactory _shootingModeFactory;

        public Weapon CurrentWeapon { get; private set; }

        public void OnDestroy()
        {
            Cleanup();
        }

        public void Initialize(GameInputProvider gameInputProvider,
            InventoryProvider inventoryProvider, 
            ShootingModeFactory shootingModeFactory,
            Player.Player owner)
        {
            _gameInputProvider = gameInputProvider;
            _inventoryProvider = inventoryProvider;
            _shootingModeFactory = shootingModeFactory;
            
            int i = 0;
            foreach (var weapon in _inventoryProvider.GameInventory.Weapons)
            {
                if (!weapon)
                {
                    photonView.RPC(nameof(SetupWeapon), RpcTarget.AllBuffered, -1, -1, "");
                    i++;
                    continue;
                }
                
                var weaponObject
                    = PhotonNetwork.Instantiate(weapon.Prefab.name, _weaponHolder.position, _weaponHolder.rotation)
                        .GetComponent<Weapon>();
                
                var newConfig = GetNewConfig(weapon);
                weaponObject.Initialize(newConfig);
                
                weaponObject.OnPlayerHit += (value) => OnPlayerHit?.Invoke(value, weaponObject.Config.Damage);
                photonView.RPC(nameof(SetupWeapon),
                    RpcTarget.AllBuffered, 
                    weaponObject.GetComponent<PhotonView>().ViewID, 
                    owner.GetComponent<PhotonView>().ViewID,
                    _inventoryProvider.GameInventory.Weapons[i].Id);
                i++;
            }
            
            _gameInputProvider.OnNumber += RPCSetWeaponByIndex;
            var index = Weapons.FindIndex(x=>x);
            RPCSetWeaponByIndex(index + 1);
        }

        public void Cleanup()
        {
            if (_gameInputProvider == null)
            {
                return;
            }
            
            _gameInputProvider.OnNumber -= RPCSetWeaponByIndex;
        }

        private void RPCSetWeaponByIndex(int index)
        {
            index--;
            index = Math.Clamp(index, 0, Weapons.Count - 1);
            var weapon = Weapons[index];
            if (!weapon)
            {
                return;
            }
            OnWeaponChanged?.Invoke(weapon);
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
            
            weapon.SetOwner(player);

            var weaponTransform = weapon.transform;
            weaponTransform.SetParent(_weaponHolder);
            weapon.gameObject.SetActive(false);
        }

        [PunRPC]
        public void SetWeaponByIndex(int index)
        {
            if (CurrentWeapon == Weapons[index])
            {
                return;
            }
            
            if (CurrentWeapon != null)
            {
                CurrentWeapon.gameObject.SetActive(false);
                CurrentWeapon.CancelAttack(true);
                CurrentWeapon.CancelAttack(false);
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