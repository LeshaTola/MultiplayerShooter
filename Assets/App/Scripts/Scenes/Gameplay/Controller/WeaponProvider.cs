using System;
using System.Collections.Generic;
using App.Scripts.Features.Input;
using App.Scripts.Features.Inventory;
using App.Scripts.Features.Inventory.Weapons;
using App.Scripts.Scenes.Gameplay.Weapons;
using Photon.Pun;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.Gameplay.Controller
{
    public class WeaponProvider : MonoBehaviour
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
            foreach (var weapon in _inventory.Weapons)
            {
                var weaponObject
                    = PhotonNetwork.Instantiate(weapon.name, Vector3.zero, Quaternion.identity)
                        .GetComponent<Weapon>();
                weaponObject.transform.SetParent(_weaponHolder);
                weaponObject.transform.localPosition = Vector3.zero;
                weaponObject.gameObject.SetActive(false);
                _weapons.Add(weaponObject);
            }
            
            _gameInputProvider.OnNumber += SetWeaponByIndex;

            SetWeaponByIndex(0);
        }

        public void Cleanup()
        {
            _gameInputProvider.OnNumber -= SetWeaponByIndex;
        }

        private void SetWeaponByIndex(int index)
        {
            index--;
            index = Math.Clamp(index, 0, _weapons.Count - 1);

            if (CurrentWeapon != null)
            {
                CurrentWeapon.gameObject.SetActive(false);
            }

            CurrentWeapon = _weapons[index];
            CurrentWeapon.gameObject.SetActive(true);
        }
    }
}