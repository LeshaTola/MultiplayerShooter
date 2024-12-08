using System.Collections.Generic;
using App.Scripts.Features.Input;
using App.Scripts.Features.Inventory;
using App.Scripts.Scenes.Gameplay.Weapons;
using Photon.Pun;
using UnityEngine;
using System;

namespace App.Scripts.Scenes.Gameplay.Controller
{
    public class WeaponProvider : MonoBehaviourPun
    {
        public event Action<Weapon> OnWeaponChanged;
        public event Action<Vector3> OnPlayerHit;
        
        [SerializeField] private Inventory _inventory;
        [SerializeField] private Transform _weaponHolder;
        
        private List<Weapon> _weapons = new();
        private GameInputProvider _gameInputProvider;
        
        public Weapon CurrentWeapon { get; private set; }

        public void Initialize(GameInputProvider gameInputProvider, Player owner)
        {
            _gameInputProvider = gameInputProvider;
            
            int i = 0;
            foreach (var weapon in _inventory.Weapons)
            {
                var weaponObject
                    = PhotonNetwork.Instantiate(weapon.Prefab.name, _weaponHolder.position, _weaponHolder.rotation)
                        .GetComponent<Weapon>();
                
                weaponObject.OnPlayerHit += (value) => OnPlayerHit?.Invoke(value);
                photonView.RPC(nameof(SetupWeapon),
                    RpcTarget.AllBuffered, 
                    weaponObject.GetComponent<PhotonView>().ViewID, 
                    owner.GetComponent<PhotonView>().ViewID,
                    i);
                i++;
            }
            
            _gameInputProvider.OnNumber += RPCSetWeaponByIndex;

            RPCSetWeaponByIndex(0);
        }

        public void Cleanup()
        {
            _gameInputProvider.OnNumber -= RPCSetWeaponByIndex;
        }

        private void RPCSetWeaponByIndex(int index)
        {
            index--;
            index = Math.Clamp(index, 0, _weapons.Count - 1);
            OnWeaponChanged?.Invoke(_weapons[index]);
            photonView.RPC(nameof(SetWeaponByIndex), RpcTarget.AllBuffered, index);
        }

        [PunRPC]
        public void SetupWeapon(int weaponViewID,int ownerId, int index)
        {
            var weaponObject = PhotonView.Find(weaponViewID).gameObject;
            var player = PhotonView.Find(ownerId).GetComponent<Player>();
            var weapon = weaponObject.GetComponent<Weapon>();

            _weapons.Add(weapon);
            weapon.Initialize(_inventory.Weapons[index], player);

            var weaponTransform = weapon.transform;
            weaponTransform.SetParent(_weaponHolder);
            weapon.gameObject.SetActive(false);
        }

        [PunRPC]
        public void SetWeaponByIndex(int index)
        {
            
            if (CurrentWeapon == _weapons[index])
            {
                return;
            }
            
            if (CurrentWeapon != null)
            {
                CurrentWeapon.gameObject.SetActive(false);
            }

            CurrentWeapon = _weapons[index];
            CurrentWeapon.gameObject.SetActive(true);
        }
    }
}