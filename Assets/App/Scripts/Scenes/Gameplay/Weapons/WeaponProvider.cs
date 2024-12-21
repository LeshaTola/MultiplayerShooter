using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Features.Input;
using App.Scripts.Features.Inventory;
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
        [SerializeField] private GlobalInventory _globalInventory;

        public List<Weapon> Weapons { get; } = new();
        private GameInputProvider _gameInputProvider;
        private InventoryProvider _inventoryProvider;

        public Weapon CurrentWeapon { get; private set; }

        public void OnDestroy()
        {
            Cleanup();
        }

        public void Initialize(GameInputProvider gameInputProvider,
            InventoryProvider inventoryProvider, Player.Player owner)
        {
            _gameInputProvider = gameInputProvider;
            _inventoryProvider = inventoryProvider;
            
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
            var weaponConfig = _globalInventory.Weapons.FirstOrDefault(x => x.Id.Equals(id));
            weapon.Initialize(weaponConfig, player);

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
                CurrentWeapon.CancelAttack();
            }

            CurrentWeapon = Weapons[index];
            CurrentWeapon.gameObject.SetActive(true);
        }
    }
}