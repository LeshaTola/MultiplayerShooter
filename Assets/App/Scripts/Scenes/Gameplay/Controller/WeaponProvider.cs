using System;
using System.Collections.Generic;
using App.Scripts.Features.Input;
using App.Scripts.Features.Inventory;
using App.Scripts.Features.Inventory.Weapons;
using App.Scripts.Scenes.Gameplay.Weapons;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.Gameplay.Controller
{
    public class WeaponProvider : MonoBehaviourPun
    {
        [SerializeField] private Inventory _inventory;
        
        [SerializeField] private Transform _weaponHolder;
        
        private List<Weapon> _weapons = new();
        private GameInputProvider _gameInputProvider;
        
        public Weapon CurrentWeapon { get; private set; }

        public void Initialize(/*Inventory inventory,*/ GameInputProvider gameInputProvider)
        {
            _gameInputProvider = gameInputProvider;
            //_inventory = inventory;
            int i = 0;
            foreach (var weapon in _inventory.Weapons)
            {
                var weaponObject
                    = PhotonNetwork.Instantiate(weapon.Prefab.name, Vector3.zero, Quaternion.identity)
                        .GetComponent<Weapon>();
                _weapons.Add(weaponObject);
                weaponObject.Initialize(weapon);
                photonView.RPC(nameof(SetupWeapon),RpcTarget.All, i);
                i++;
            }
            
            _gameInputProvider.OnNumber += NetworkSetWeaponByIndex;

            NetworkSetWeaponByIndex(0);
        }

        public void Cleanup()
        {
            _gameInputProvider.OnNumber -= SetWeaponByIndex;
        }

        [PunRPC]
        public void SetupWeapon(int  index)
        {
            var weaponObject = _weapons[index];
            var weaponTransform = weaponObject.transform;
            weaponTransform.SetParent(_weaponHolder);
            weaponTransform.localPosition = Vector3.zero;
            weaponTransform.localRotation = Quaternion.identity;
            weaponObject.gameObject.SetActive(false);
        }

        private void NetworkSetWeaponByIndex(int index)
        {
            photonView.RPC(nameof(SetWeaponByIndex),RpcTarget.All, index);
        }
        
        [PunRPC]
        public void SetWeaponByIndex(int index)
        {
            index--;
            index = Math.Clamp(index, 0, _weapons.Count - 1);
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